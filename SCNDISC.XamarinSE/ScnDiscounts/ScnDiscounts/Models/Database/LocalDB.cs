using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.Database.Tables;
using ScnDiscounts.Models.WebService.MongoDB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ScnDiscounts.Models.Database
{
    public class LocalDb
    {
        private readonly SQLiteConnection _database;

        public const byte StrAppointmentTitle = 1;
        public const byte StrAppointmentDescription = 2;
        public const byte StrAppointmentAddress = 3;
        public const byte StrAppointmentCategory = 4;

        public LocalDb()
        {
            try
            {
                _database = DependencyService.Get<IClientDatabase>().GetSQLiteConnection("data.db3");

                _database.CreateTable<Discount>();
                _database.CreateTable<LangString>();
                _database.CreateTable<DiscountsStrings>();
                _database.CreateTable<Contact>();
                _database.CreateTable<Category>();
                _database.CreateTable<DiscountCategory>();
                _database.CreateTable<WebAddress>();
                _database.CreateTable<GalleryImage>();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);
            }
        }

        private const string ImgFileExtension = ".png";
        private const string PostfixLogoFileName = "_logo" + ImgFileExtension;
        private const string PostfixImgFileName = "_img" + ImgFileExtension;

        private static async Task SaveImage(string fileName, string base64Image)
        {
            await IsolatedStorageHelper.SaveBase64Async(fileName, base64Image);
        }

        private static async Task SaveImage(string fileName, Stream streamImage)
        {
            await IsolatedStorageHelper.SaveStreamAsync(fileName, streamImage);
        }

        private static void DeleteImage(string fileName)
        {
            IsolatedStorageHelper.DeleteFile(fileName);
        }

        private static void DeleteGallery(IEnumerable<string> images)
        {
            foreach (var image in images)
            {
                IsolatedStorageHelper.DeleteFile(image);
            }
        }

        private void SaveCategory(DeserializeCategoryItem categoryItem, Category categoryDbData)
        {
            _database.BeginTransaction();

            if (categoryDbData != null)
            {
                _database.Table<DiscountsStrings>()
                    .Where(i => i.OwnerId == categoryDbData.Id && i.Appointment == StrAppointmentCategory)
                    .Select(i => i.LangStringId).ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                _database.Table<DiscountsStrings>().Delete(i =>
                    i.OwnerId == categoryDbData.Id && i.Appointment == StrAppointmentCategory);

                categoryDbData.DocumentId = categoryItem.Id;
                categoryDbData.Color = categoryItem.Color;
                categoryDbData.Modified = categoryItem.Modified ?? DateTime.UtcNow;

                _database.Update(categoryDbData);
            }
            else
            {
                categoryDbData = new Category
                { 
                    DocumentId = categoryItem.Id,
                    Color = categoryItem.Color,
                    Modified = categoryItem.Modified ?? DateTime.UtcNow
                };

                _database.Insert(categoryDbData);
            }

            #region save children

            if (categoryItem.Name != null)
            {
                foreach (var name in categoryItem.Name)
                {
                    var langStringRec = new LangString
                    {
                        LanguageCode = name.Lan.ToUpper(),
                        Text = name.LocText
                    };
                    _database.Insert(langStringRec);

                    var discountsStringsRec = new DiscountsStrings
                    {
                        Appointment = StrAppointmentCategory,
                        OwnerId = categoryDbData.Id,
                        LangStringId = langStringRec.Id
                    };
                    _database.Insert(discountsStringsRec);
                }
            }

            #endregion

            _database.Commit();
        }

        private async Task SaveDiscount(DeserializeBranchItem branchItem, Discount discountDbData)
        {
            string logoFileName;
            if (!string.IsNullOrEmpty(branchItem.Icon))
            {
                logoFileName = branchItem.Id + PostfixLogoFileName;
                await SaveImage(logoFileName, branchItem.Icon);
            }
            else
                logoFileName = null;

            _database.BeginTransaction();

            if (discountDbData != null)
            {
                _database.Table<WebAddress>().Delete(i => i.DiscountId == discountDbData.Id);
                _database.Table<DiscountCategory>().Delete(i => i.DiscountId == discountDbData.Id);
                _database.Table<DiscountsStrings>()
                    .Where(i => i.OwnerId == discountDbData.Id &&
                                (i.Appointment == StrAppointmentTitle || i.Appointment == StrAppointmentDescription))
                    .Select(i => i.LangStringId).ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                _database.Table<DiscountsStrings>().Delete(i =>
                    i.OwnerId == discountDbData.Id &&
                    (i.Appointment == StrAppointmentTitle || i.Appointment == StrAppointmentDescription));

                var images = _database.Table<GalleryImage>().Where(i => i.DiscountId == discountDbData.Id)
                    .Select(i => i.FileName);
                DeleteGallery(images);
                _database.Table<GalleryImage>().Delete(i => i.DiscountId == discountDbData.Id);

                discountDbData.DocumentId = branchItem.Id;
                discountDbData.LogoFileName = logoFileName;
                discountDbData.ImageFileName = null;
                discountDbData.PercentValue = branchItem.Discounts.FirstOrDefault()?.Name?.FirstOrDefault()?.LocText;
                discountDbData.DiscountType = branchItem.Discounts.FirstOrDefault()?.DiscountType;
                discountDbData.Modified = branchItem.Modified ?? DateTime.UtcNow;

                _database.Update(discountDbData);
            }
            else
            {
                discountDbData = new Discount
                {
                    DocumentId = branchItem.Id,
                    LogoFileName = logoFileName,
                    ImageFileName = null,
                    PercentValue = branchItem.Discounts.FirstOrDefault()?.Name?.FirstOrDefault()?.LocText,
                    DiscountType = branchItem.Discounts.FirstOrDefault()?.DiscountType,
                    Modified = branchItem.Modified ?? DateTime.UtcNow
                };

                _database.Insert(discountDbData);
            }

            #region save children

            if (branchItem.Name != null)
            {
                foreach (var name in branchItem.Name)
                {
                    var langStringRec = new LangString
                    {
                        LanguageCode = name.Lan.ToUpper(),
                        Text = name.LocText
                    };
                    _database.Insert(langStringRec);

                    var discountsStringsRec = new DiscountsStrings
                    {
                        Appointment = StrAppointmentTitle,
                        OwnerId = discountDbData.Id,
                        LangStringId = langStringRec.Id
                    };
                    _database.Insert(discountsStringsRec);
                }
            }

            if (branchItem.Description != null)
            {
                foreach (var name in branchItem.Description)
                {
                    var langStringRec = new LangString
                    {
                        LanguageCode = name.Lan.ToUpper(),
                        Text = name.LocText
                    };
                    _database.Insert(langStringRec);

                    var discountsStringsRec = new DiscountsStrings
                    {
                        Appointment = StrAppointmentDescription,
                        OwnerId = discountDbData.Id,
                        LangStringId = langStringRec.Id
                    };
                    _database.Insert(discountsStringsRec);
                }
            }

            if (branchItem.CategoryIds != null)
            {
                foreach (var category in branchItem.CategoryIds)
                {
                    var categorieRec = new DiscountCategory
                    {
                        DiscountId = discountDbData.Id,
                        CategoryId = category
                    };
                    _database.Insert(categorieRec);
                }
            }

            if (branchItem.WebAddresses != null)
            {
                foreach (var webAddress in branchItem.WebAddresses)
                {
                    var webAddressRec = new WebAddress
                    {
                        DiscountId = discountDbData.Id,
                        Category = webAddress.Category,
                        Url = webAddress.Url
                    };
                    _database.Insert(webAddressRec);
                }
            }

            #endregion

            _database.Commit();
        }

        private void SaveContact(DeserializeBranchItem branchItem, Contact contactDbData)
        {
            var discountDbData = _database.Table<Discount>().FirstOrDefault(i => i.DocumentId == branchItem.PartnerId);
            if (discountDbData == null)
                return;

            var format = new NumberFormatInfo
            {
                NumberGroupSeparator = ",",
                NumberDecimalSeparator = "."
            };

            _database.BeginTransaction();

            if (contactDbData != null)
            {
                _database.Table<DiscountsStrings>()
                    .Where(i => i.OwnerId == contactDbData.Id && i.Appointment == StrAppointmentAddress)
                    .Select(i => i.LangStringId).ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                _database.Table<DiscountsStrings>().Delete(i =>
                    i.OwnerId == contactDbData.Id && i.Appointment == StrAppointmentAddress);

                contactDbData.DiscountId = discountDbData.Id;
                contactDbData.DocumentId = branchItem.Id;
                contactDbData.Latitude = double.Parse(branchItem.Location.Coordinates.Latitude, format);
                contactDbData.Longitude = double.Parse(branchItem.Location.Coordinates.Longitude, format);
                contactDbData.Modified = branchItem.Modified ?? DateTime.UtcNow;
                contactDbData.Phone1 = branchItem.Phones?.ElementAtOrDefault(0)?.Number;
                contactDbData.Phone2 = branchItem.Phones?.ElementAtOrDefault(1)?.Number;

                _database.Update(contactDbData);
            }
            else
            {
                contactDbData = new Contact
                {
                    DiscountId = discountDbData.Id,
                    DocumentId = branchItem.Id,
                    Latitude = double.Parse(branchItem.Location.Coordinates.Latitude, format),
                    Longitude = double.Parse(branchItem.Location.Coordinates.Longitude, format),
                    Modified = branchItem.Modified ?? DateTime.UtcNow,
                    Phone1 = branchItem.Phones?.ElementAtOrDefault(0)?.Number,
                    Phone2 = branchItem.Phones?.ElementAtOrDefault(1)?.Number
                };

                _database.Insert(contactDbData);
            }

            #region save children

            if (branchItem.Address != null)
            {
                foreach (var address in branchItem.Address)
                {
                    var langStringRec = new LangString
                    {
                        LanguageCode = address.Lan.ToUpper(),
                        Text = address.LocText
                    };
                    _database.Insert(langStringRec);

                    var discountsStringsRec = new DiscountsStrings
                    {
                        Appointment = StrAppointmentAddress,
                        OwnerId = contactDbData.Id,
                        LangStringId = langStringRec.Id
                    };
                    _database.Insert(discountsStringsRec);
                }
            }

            #endregion

            _database.Commit();
        }

        public async Task UpdateDiscountImage(string documentId, Stream streamImage)
        {
            var discountDbData = _database.Table<Discount>().FirstOrDefault(i => i.DocumentId == documentId);
            if (discountDbData == null)
                return;

            string imgFileName;
            if (streamImage.Length > 0)
            {
                imgFileName = discountDbData.DocumentId + PostfixImgFileName;
                await SaveImage(imgFileName, streamImage);
            }
            else
                imgFileName = null;

            discountDbData.ImageFileName = imgFileName;

            _database.BeginTransaction();

            _database.Update(discountDbData);

            _database.Commit();
        }

        public async Task UpdateDiscountGalleryImage(string documentId, string imageId, Stream streamImage)
        {
            var discountDbData = _database.Table<Discount>().FirstOrDefault(i => i.DocumentId == documentId);
            if (discountDbData == null)
                return;

            if (streamImage.Length > 0)
            {
                var imgFileName = imageId + ImgFileExtension;
                await SaveImage(imgFileName, streamImage);

                var galleryImage = new GalleryImage
                {
                    DiscountId = discountDbData.Id,
                    FileName = imgFileName
                };

                _database.BeginTransaction();

                _database.Insert(galleryImage);

                _database.Commit();
            }
        }

        public DiscountDetailData LoadDiscountDetail(string documentId)
        {
            var discountRec = _database.Table<Discount>().FirstOrDefault(i => i.DocumentId == documentId);
            if (discountRec == null)
                return null;

            var discountCategories = _database.Table<DiscountCategory>().ToList();
            var discountsStrings = _database.Table<DiscountsStrings>().ToList();
            var langStrings = _database.Table<LangString>().ToList();
            var webAddresses = _database.Table<WebAddress>().ToList();
            var galleryImages = _database.Table<GalleryImage>().ToList();

            var nameRec = discountsStrings
                .Where(i => i.OwnerId == discountRec.Id && i.Appointment == StrAppointmentTitle)
                .Join(langStrings, i => i.LangStringId, i => i.Id, (ds, ls) => ls)
                .FirstOrDefault(i => i.LanguageCode == AppParameters.Config.SystemLang.LangEnumToCode());

            var descriptionRec = discountsStrings
                .Where(i => i.OwnerId == discountRec.Id && i.Appointment == StrAppointmentDescription)
                .Join(langStrings, i => i.LangStringId, i => i.Id, (ds, ls) => ls)
                .FirstOrDefault(i => i.LanguageCode == AppParameters.Config.SystemLang.LangEnumToCode());

            var categories = discountCategories.Where(i => i.DiscountId == discountRec.Id)
                .Join(AppData.Discount.CategoryCollection, i => i.CategoryId, i => i.DocumentId, (dc, c) => c)
                .ToList();

            var webAddressRecs = webAddresses.Where(i => i.DiscountId == discountRec.Id).OrderBy(i => i.Category)
                .Select(i => new WebAddressData(i)).ToList();

            var galleryImagesRecs = galleryImages.Where(i => i.DiscountId == discountRec.Id).OrderBy(i => i.Id)
                .Select(i => new GalleryImageData(i)).ToList();

            var branches = new List<DiscountDetailBranchData>();
            var contactList = _database.Table<Contact>().Where(i => i.DiscountId == discountRec.Id).ToList();
            foreach (var contactRec in contactList)
            {
                var addressRec = discountsStrings
                    .Where(i => i.OwnerId == contactRec.Id && i.Appointment == StrAppointmentAddress)
                    .Join(langStrings, i => i.LangStringId, i => i.Id, (ds, ls) => ls)
                    .FirstOrDefault(i => i.LanguageCode == AppParameters.Config.SystemLang.LangEnumToCode());

                var discountDetailBranchData = new DiscountDetailBranchData
                {
                    DocumentId = contactRec.DocumentId,
                    Latitude = contactRec.Latitude,
                    Longitude = contactRec.Longitude,
                    Address = addressRec?.Text,
                    Phone1 = contactRec.Phone1,
                    Phone2 = contactRec.Phone2
                };

                branches.Add(discountDetailBranchData);
            }

            return new DiscountDetailData
            {
                Persent = discountRec.PercentValue,
                DiscountType = discountRec.DiscountType.GetDiscountTypeName(),
                ImageFileName = discountRec.ImageFileName,
                Title = nameRec?.Text,
                Description = descriptionRec?.Text,
                CategoryList = categories,
                WebAddresses = webAddressRecs,
                BranchList = branches,
                GalleryImages = galleryImagesRecs
            };
        }

        public List<string> GetDiscountsWithoutImages()
        {
            return _database.Table<Discount>().Where(i => i.ImageFileName == null).Select(i => i.DocumentId).ToList();
        }

        public List<string> GetDiscountsWithoutGallery()
        {
            var discounts = _database.Table<GalleryImage>().Select(i => i.DiscountId).Distinct().ToList();
            return _database.Table<Discount>().Where(i => !discounts.Contains(i.Id)).Select(i => i.DocumentId).ToList();
        }

        public DateTime? GetCategoryLastSyncDate()
        {
            var result = _database.Table<Category>().Select(i => i.Modified).DefaultIfEmpty().Max();

            if (result.HasValue)
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);

            return result;
        }

        public DateTime? GetDiscountLastSyncDate()
        {
            var result = _database.Table<Discount>().Select(i => i.Modified).DefaultIfEmpty().Max();

            if (result.HasValue)
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);

            return result;
        }

        public DateTime? GetContactLastSyncDate()
        {
            var result = _database.Table<Contact>().Select(i => i.Modified).DefaultIfEmpty().Max();

            if (result.HasValue)
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);

            return result;
        }

        public async Task SyncCategory(DeserializeCategoryItem item)
        {
            var category = _database.Table<Category>().FirstOrDefault(i => i.DocumentId == item.Id);

            if (item.IsDeleted)
            {
                if (category != null)
                {
                    _database.BeginTransaction();

                    _database.Table<Category>().Delete(i => i.Id == category.Id);
                    _database.Table<DiscountCategory>().Delete(i => i.CategoryId == category.DocumentId);
                    _database.Table<DiscountsStrings>()
                        .Where(i => i.OwnerId == category.Id && i.Appointment == StrAppointmentCategory)
                        .Select(i => i.LangStringId).ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                    _database.Table<DiscountsStrings>().Delete(i =>
                        i.OwnerId == category.Id && i.Appointment == StrAppointmentCategory);

                    _database.Commit();
                }
            }
            else
                await Task.Run(() => SaveCategory(item, category));
        }


        public async Task SyncDiscount(DeserializeBranchItem item)
        {
            var discount = _database.Table<Discount>().FirstOrDefault(i => i.DocumentId == item.Id);

            if (item.IsDeleted)
            {
                if (discount != null)
                {
                    _database.BeginTransaction();

                    _database.Table<Discount>().Delete(i => i.Id == discount.Id);
                    _database.Table<WebAddress>().Delete(i => i.DiscountId == discount.Id);
                    _database.Table<DiscountCategory>().Delete(i => i.DiscountId == discount.Id);
                    _database.Table<DiscountsStrings>()
                        .Where(i => i.OwnerId == discount.Id &&
                                    (i.Appointment == StrAppointmentTitle ||
                                     i.Appointment == StrAppointmentDescription)).Select(i => i.LangStringId)
                        .ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                    _database.Table<DiscountsStrings>().Delete(i =>
                        i.OwnerId == discount.Id && (i.Appointment == StrAppointmentTitle ||
                                                     i.Appointment == StrAppointmentDescription));

                    var logoFileName = discount.DocumentId + PostfixLogoFileName;
                    DeleteImage(logoFileName);

                    var imgFileName = discount.DocumentId + PostfixImgFileName;
                    DeleteImage(imgFileName);

                    var images = _database.Table<GalleryImage>().Where(i => i.DiscountId == discount.Id)
                        .Select(i => i.FileName);
                    DeleteGallery(images);
                    _database.Table<GalleryImage>().Delete(i => i.DiscountId == discount.Id);

                    _database.Commit();
                }
            }
            else
                await SaveDiscount(item, discount);
        }

        public async Task SyncContact(DeserializeBranchItem item)
        {
            var contact = _database.Table<Contact>().FirstOrDefault(i => i.DocumentId == item.Id);

            if (item.IsDeleted)
            {
                if (contact != null)
                {
                    _database.BeginTransaction();

                    _database.Table<Contact>().Delete(i => i.Id == contact.Id);
                    _database.Table<DiscountsStrings>()
                        .Where(i => i.OwnerId == contact.Id && i.Appointment == StrAppointmentAddress)
                        .Select(i => i.LangStringId).ForEach(i => _database.Table<LangString>().Delete(j => j.Id == i));
                    _database.Table<DiscountsStrings>().Delete(i =>
                        i.OwnerId == contact.Id && i.Appointment == StrAppointmentAddress);

                    _database.Commit();
                }
            }
            else
                await Task.Run(() => SaveContact(item, contact));
        }

        public void LoadData()
        {
            var discountsTable = _database.Table<Discount>().ToList();
            var contactsTable = _database.Table<Contact>().ToList();
            var categoriesTable = _database.Table<Category>().ToList();
            var discountCategoriesTable = _database.Table<DiscountCategory>().ToList();
            var discountsStringsTable = _database.Table<DiscountsStrings>().ToList();
            var langStringsTable = _database.Table<LangString>().ToList();

            LoadCategories(categoriesTable, discountsStringsTable, langStringsTable);
            LoadMapData(discountsTable, contactsTable, discountCategoriesTable, discountsStringsTable, langStringsTable);
            LoadDiscounts(discountsTable, discountCategoriesTable, discountsStringsTable, langStringsTable);
        }

        private static void LoadCategories(
            IEnumerable<Category> categoriesTable,
            IReadOnlyCollection<DiscountsStrings> discountsStringsTable,
            IReadOnlyCollection<LangString> langStringsTable)
        {
            var categoryCollection = new List<CategoryData>();

            foreach (var category in categoriesTable)
            {
                var categoryData = new CategoryData
                {
                    Id = category.Id,
                    DocumentId = category.DocumentId,
                    Color = category.Color
                };

                var nameList = discountsStringsTable
                    .Where(i => i.OwnerId == category.Id && i.Appointment == StrAppointmentCategory)
                    .Join(langStringsTable, i => i.LangStringId, i => i.Id, (ds, ls) => ls);

                foreach (var nameRec in nameList)
                    categoryData.SetName(nameRec.LanguageCode, nameRec.Text);

                categoryCollection.Add(categoryData);
            }

            AppData.Discount.CategoryCollection = categoryCollection;
        }

        private static void LoadMapData(
            IEnumerable<Discount> discountsTable,
            IEnumerable<Contact> contactsTable,
            IReadOnlyCollection<DiscountCategory> discountCategoriesTable, 
            IReadOnlyCollection<DiscountsStrings> discountsStringsTable,
            IReadOnlyCollection<LangString> langStringsTable)
        {
            var mapPinCollection = new List<MapPinData>();

            var contactList = contactsTable.Join(discountsTable, i => i.DiscountId, i => i.Id,
                (c, d) => new
                {
                    d.Id,
                    c.DocumentId,
                    PartnerId = d.DocumentId,
                    c.Latitude,
                    c.Longitude,
                    d.PercentValue,
                    d.DiscountType
                });

            foreach (var contactRec in contactList)
            {
                var categories = discountCategoriesTable.Where(i => i.DiscountId == contactRec.Id)
                    .Join(AppData.Discount.CategoryCollection, i => i.CategoryId, i => i.DocumentId, (dc, c) => c)
                    .ToList();

                var mapPinData = new MapPinData
                {
                    Id = contactRec.DocumentId,
                    PartnerId = contactRec.PartnerId,
                    Discount = contactRec.PercentValue,
                    DiscountType = contactRec.DiscountType.GetDiscountTypeName(),
                    Latitude = contactRec.Latitude,
                    Longitude = contactRec.Longitude,
                    CategoryList = categories,
                    PrimaryCategory = categories.FirstOrDefault()
                };

                var nameList = discountsStringsTable
                    .Where(i => i.OwnerId == contactRec.Id && i.Appointment == StrAppointmentTitle)
                    .Join(langStringsTable, i => i.LangStringId, i => i.Id, (ds, ls) => ls);

                foreach (var nameRec in nameList)
                    mapPinData.SetName(nameRec.LanguageCode, nameRec.Text);

                mapPinCollection.Add(mapPinData);
            }

            AppData.Discount.MapPinCollection = mapPinCollection;
        }

        public void LoadDiscounts(
            IEnumerable<Discount> discountsTable,
            IReadOnlyCollection<DiscountCategory> discountCategoriesTable,
            IReadOnlyCollection<DiscountsStrings> discountsStringsTable,
            IReadOnlyCollection<LangString> langStringsTable)
        {
            var discountCollection = new List<DiscountData>();

            foreach (var discount in discountsTable)
            {
                var categories = discountCategoriesTable.Where(i => i.DiscountId == discount.Id)
                    .Join(AppData.Discount.CategoryCollection, i => i.CategoryId, i => i.DocumentId, (dc, c) => c)
                    .ToList();

                var discountData = new DiscountData
                {
                    DocumentId = discount.DocumentId,
                    DiscountPercent = discount.PercentValue,
                    DiscountType = discount.DiscountType.GetDiscountTypeName(),
                    LogoFileName = discount.LogoFileName,
                    CategoryList = categories
                };

                var nameList = discountsStringsTable
                    .Where(i => i.OwnerId == discount.Id && i.Appointment == StrAppointmentTitle)
                    .Join(langStringsTable, i => i.LangStringId, i => i.Id, (ds, ls) => ls);

                foreach (var nameRec in nameList)
                    discountData.SetName(nameRec.LanguageCode, nameRec.Text);

                var descrList = discountsStringsTable
                    .Where(i => i.OwnerId == discount.Id && i.Appointment == StrAppointmentDescription)
                    .Join(langStringsTable, i => i.LangStringId, i => i.Id, (ds, ls) => ls);

                foreach (var descrRec in descrList)
                    discountData.SetDescription(descrRec.LanguageCode, descrRec.Text);

                discountCollection.Add(discountData);
            }

            AppData.Discount.DiscountCollection = discountCollection;
        }
    }
}
