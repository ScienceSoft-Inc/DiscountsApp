using SQLite;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Models.Database.Tables;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.WebService.MongoDB;
using System.Globalization;
using ScnDiscounts.Helpers;
using System.Threading.Tasks;

namespace ScnDiscounts.Models.Database
{
    public class LocalDB
    {
        static object locker = new object();
        private SQLiteConnection database;

        public const byte StrAppointmentTitle = 1;
        public const byte StrAppointmentDescription = 2;
        public const byte StrAppointmentAddress = 3;

        public LocalDB()
        {
            try
            {
                var databasePath = DependencyService.Get<IClientDatabase>().GetPath("data.db3");
                database = new SQLiteConnection(databasePath);

                database.CreateTable<Discount>();
                database.CreateTable<LangString>();
                database.CreateTable<DiscountsStrings>();
                database.CreateTable<Contact>();
                database.CreateTable<Categorie>();
            }
            catch(Exception ex)
            {

            }
        }

        private const string PostfixLogoFileName = "_logo.png";
        private const string PostfixImgFileName = "_img.png";

        public async Task SaveImage(string fileName, string streamImage)
        {
            await IsolatedStorageHelper.SaveBase64Async(fileName, streamImage);
        }

        public async Task SaveDiscount(DiscountData discount)
        {
            database.BeginTransaction();
            string logoFileName = discount.DocumentId + PostfixLogoFileName;
            
            await SaveImage(logoFileName, discount.Icon);

            Discount discountDBData = new Discount 
            { 
                UrlAddress = discount.UrlAddress, 
                IsFullDescription = false,
                DocumentId = discount.DocumentId,
                LogoFileName = logoFileName
            };
            discountDBData.PercentValue = discount.DiscountPercent;

            database.Insert(discountDBData);

            //name
            foreach (var name in discount.NameList)
            {
                LangString langStringRec = new LangString { LanguageCode = LanguageHelper.LangEnumToCode(name.Key).ToUpper(), Text = name.Value };
                database.Insert(langStringRec);

                DiscountsStrings discountsStringsRec = new DiscountsStrings {Appointment = StrAppointmentTitle, OwnerId = discountDBData.Id, LangStringId = langStringRec.Id };
                database.Insert(discountsStringsRec);
            }

            //description
            foreach (var name in discount.DescriptionList)
            {
                LangString langStringRec = new LangString { LanguageCode = LanguageHelper.LangEnumToCode(name.Key).ToUpper(), Text = name.Value };
                database.Insert(langStringRec);

                DiscountsStrings discountsStringsRec = new DiscountsStrings { Appointment = StrAppointmentDescription, OwnerId = discountDBData.Id, LangStringId = langStringRec.Id };
                database.Insert(discountsStringsRec);
            }

            //categories
            foreach (var categorie in discount.CategorieList)
            {
                Categorie categorieRec = new Categorie { DiscountId = discountDBData.Id, TypeCode = categorie.TypeCode };
                database.Insert(categorieRec);
            }

            database.Commit();
        }

        public async Task UpdateDiscount(DeserializeBranchItem branchItem)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ",";
            format.NumberDecimalSeparator = ".";

            Discount discountDBData = database.Table<Discount>().FirstOrDefault(x => x.DocumentId == branchItem.PartnerId);
            if (discountDBData == null)
                return;

            database.BeginTransaction();

            if (!String.IsNullOrWhiteSpace(branchItem.Image))
            {
                string imgFileName = discountDBData.DocumentId + PostfixImgFileName;
                await SaveImage(imgFileName, branchItem.Image);
                discountDBData.ImageFileName = imgFileName;
                discountDBData.IsFullDescription = true;
                database.Update(discountDBData);

                var discount = AppData.Discount.DiscountCollection.FirstOrDefault<DiscountData>(x => x.DocumentId == discountDBData.DocumentId);
                if (discount != null)
                    discount.IsFullDescription = true;
            }

            Contact contactDBData = database.Table<Contact>().FirstOrDefault(x => x.DiscountId == discountDBData.Id && x.DocumentId == branchItem.Id);

            if (contactDBData == null)
            {
                contactDBData = new Contact();
                database.Insert(contactDBData);
            }

            contactDBData.DiscountId = discountDBData.Id;
            contactDBData.DocumentId = branchItem.Id;
            contactDBData.Latitude = Double.Parse(branchItem.Location.Coordinates.Latitude, format);
            contactDBData.Longitude = Double.Parse(branchItem.Location.Coordinates.Longitude, format);

            if (branchItem.Address != null)
                foreach (var address in branchItem.Address)
                {
                    LangString langStringRec = new LangString { LanguageCode = address.Lan.ToUpper(), Text = address.LocText };
                    database.Insert(langStringRec);

                    DiscountsStrings discountsStringsRec = new DiscountsStrings { Appointment = StrAppointmentAddress, OwnerId = contactDBData.Id, LangStringId = langStringRec.Id };
                    database.Insert(discountsStringsRec);
                }

            if (branchItem.Phones != null)
            {
                if (branchItem.Phones.Count > 0)
                    contactDBData.Phone1 = branchItem.Phones[0].Number;

                if (branchItem.Phones.Count > 1)
                    contactDBData.Phone2 = branchItem.Phones[1].Number;
            }

            database.Update(contactDBData);

            database.Commit();
        }

        public void LoadDiscount()
        {
            AppData.Discount.DiscountCollection.Clear();

            foreach (var discountRec in database.Table<Discount>())
            {
                DiscountData discountData = new DiscountData();
                discountData.DocumentId = discountRec.DocumentId;
                discountData.DiscountPercent = discountRec.PercentValue;
                discountData.IsFullDescription = discountRec.IsFullDescription;
                discountData.LogoFileName = discountRec.LogoFileName;

                var categoryList = from c in database.Table<Categorie>()
                                   where c.DiscountId == discountRec.Id
                                   select c;
                foreach (var categoryrec in categoryList)
                    discountData.CategorieList.Add(new CategorieData { TypeCode = categoryrec.TypeCode });

                var nameList = (from ds in database.Table<DiscountsStrings>()
                                from ls in database.Table<LangString>()
                                where ds.LangStringId == ls.Id
                                where ds.OwnerId == discountRec.Id
                                where ds.Appointment == StrAppointmentTitle
                                select new { ls.Text, ls.LanguageCode }).ToList();

                foreach (var nameRec in nameList)
                    discountData.SetName(nameRec.LanguageCode, nameRec.Text);

                var descrList = (from ds in database.Table<DiscountsStrings>()
                                from ls in database.Table<LangString>()
                                where ds.LangStringId == ls.Id
                                where ds.OwnerId == discountRec.Id
                                where ds.Appointment == StrAppointmentDescription
                                select new { ls.Text, ls.LanguageCode }).ToList();

                foreach (var descrRec in descrList)
                    discountData.SetDescription(descrRec.LanguageCode, descrRec.Text);

                AppData.Discount.DiscountCollection.Add(discountData);
            }
        }

        public DiscountDetailData LoadDiscountDetail(string ID)
        {
            DiscountDetailData discountDetail = new DiscountDetailData();

            var discountRec = (from d in database.Table<Discount>()
                               where d.DocumentId == ID
                               select d).FirstOrDefault();
            if (discountRec == null)
                return null;

            discountDetail.Persent = discountRec.PercentValue;
            discountDetail.UrlAddress = discountRec.UrlAddress;
            discountDetail.LogoFileName = discountRec.LogoFileName;
            discountDetail.ImageFileName = discountRec.ImageFileName;

            var titleRec =  (from ds in database.Table<DiscountsStrings>()
                            from ls in database.Table<LangString>()
                            where ds.LangStringId == ls.Id
                            where ds.OwnerId == discountRec.Id
                            where ds.Appointment == StrAppointmentTitle
                            where ls.LanguageCode == LanguageHelper.LangEnumToCode(AppParameters.Config.SystemLang)
                            select new { ls.Text }).FirstOrDefault();

            if (titleRec != null)
                discountDetail.Title = titleRec.Text;

            var descriptionRec = (from ds in database.Table<DiscountsStrings>()
                               from ls in database.Table<LangString>()
                               where ds.LangStringId == ls.Id
                               where ds.OwnerId == discountRec.Id
                               where ds.Appointment == StrAppointmentDescription
                               where ls.LanguageCode == LanguageHelper.LangEnumToCode(AppParameters.Config.SystemLang)
                               select new { ls.Text }).FirstOrDefault();
            if (descriptionRec != null)
                discountDetail.Description = descriptionRec.Text;

            var categoryList = from c in database.Table<Categorie>()
                               where c.DiscountId == discountRec.Id
                               select c;
            foreach (var categoryrec in categoryList)
                discountDetail.CategorieList.Add(new CategorieData { TypeCode = categoryrec.TypeCode });

            var contactList = from c in database.Table<Contact>()
                              where c.DiscountId == discountRec.Id
                              select c;

            foreach (var contactRec in contactList)
            {
                DiscountDetailBranchData discountDetailBranchData = new DiscountDetailBranchData();
                discountDetailBranchData.DocumentId = contactRec.DocumentId;
                discountDetailBranchData.Latitude = contactRec.Latitude;
                discountDetailBranchData.Longitude = contactRec.Longitude;

                var addressRec = (from ds in database.Table<DiscountsStrings>()
                                      from ls in database.Table<LangString>()
                                      where ds.LangStringId == ls.Id
                                      where ds.OwnerId == contactRec.Id
                                      where ds.Appointment == StrAppointmentAddress
                                      where ls.LanguageCode == LanguageHelper.LangEnumToCode(AppParameters.Config.SystemLang)
                                      select new { ls.Text }).FirstOrDefault();
                if (addressRec != null)
                    discountDetailBranchData.Address = addressRec.Text;
                
                discountDetailBranchData.PhoneList.Add(contactRec.Phone1);
                discountDetailBranchData.PhoneList.Add(contactRec.Phone2);
                discountDetail.BranchList.Add(discountDetailBranchData);
            }
            return discountDetail;
        }

        public void LoadMapData()
        {
            AppData.Discount.MapPinCollection.Clear();

            var contactList = from co in database.Table<Contact>()
                          from d in database.Table<Discount>()
                          where co.DiscountId == d.Id
                          select new
                          {
                              d.Id,
                              DocumentId = co.DocumentId,
                              PartnerId = d.DocumentId,
                              co.Latitude,
                              co.Longitude,
                              d.PercentValue,
                          };

            foreach (var contactRec in contactList)
            {
                MapPinData mapPinData = new MapPinData();
                mapPinData.Id = contactRec.DocumentId;
                mapPinData.PartnerId = contactRec.PartnerId;
                mapPinData.Discount = contactRec.PercentValue;
                mapPinData.Latitude = contactRec.Latitude;
                mapPinData.Longitude = contactRec.Longitude;


                var categoryList = from c in database.Table<Categorie>()
                                   where c.DiscountId == contactRec.Id
                                   select c;
                foreach (var categoryrec in categoryList)
                    mapPinData.CategorieList.Add(new CategorieData { TypeCode = categoryrec.TypeCode });
               
                if (mapPinData.CategorieList.Count > 0)
                    mapPinData.PrimaryCategory = mapPinData.CategorieList[0];
                else
                    mapPinData.PrimaryCategory = new CategorieData { TypeCode = -1 };

                var nameList = (from ds in database.Table<DiscountsStrings>()
                               from ls in database.Table<LangString>()
                               where ds.LangStringId == ls.Id
                               where ds.OwnerId == contactRec.Id
                               where ds.Appointment == StrAppointmentTitle
                               select new { ls.Text, ls.LanguageCode }).ToList();

                foreach (var nameRec in nameList)
                    mapPinData.SetName( nameRec.LanguageCode, nameRec.Text);

                AppData.Discount.MapPinCollection.Add(mapPinData);
            }
        }

        public void DiscountClean()
        {
            database.DeleteAll<Discount>();
            database.DeleteAll<LangString>();
            database.DeleteAll<DiscountsStrings>();
            database.DeleteAll<Contact>();
            database.DeleteAll<Categorie>();
        }
    }
}
