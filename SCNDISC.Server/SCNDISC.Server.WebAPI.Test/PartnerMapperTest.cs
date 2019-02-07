using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GeoJsonObjectModel;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Mapper;
using SCNDISC.Server.WebAPI.Models.Partner;

namespace SCNDISC.Server.WebAPI.Test
{
    [TestClass]
    public class PartnerMapperTest
    {
        private const char CoordSeparator = ',';
        private readonly CategoryModel[] _categoryModels = {
		    new CategoryModel
		    {
			    Color = "#123",
			    Id = "CategoryIds1",
			    Name_Ru = "Category1Ru",
			    Name_En = "Category1En"
		    },
		    new CategoryModel
		    {
			    Color = "#234",
			    Id = "CategoryIds2",
			    Name_Ru = "Category2Ru",
			    Name_En = "Category2En"
		    },
		    new CategoryModel
		    {
			    Color = "#345",
			    Id = "CategoryIds3",
			    Name_Ru = "Category3Ru",
			    Name_En = "Category3En"
		    }
	    };

        [TestMethod]
        public void MapToBranch()
        {
            // Arrange
            var partner = CreatePartnerModel();

            // Act
            var branches = Mapper.PartnerMapper.MapToBranches(partner);

            //Assert
            for (var i=0; i< branches.Count(); i++)
            {
                var branch = branches.ElementAt(i);
                Assert.AreEqual(branch.PartnerId, partner.Id);
                Assert.AreEqual(branch.Comment, partner.Comment);
                Assert.AreEqual(branch.Icon, partner.Logo);
                Assert.AreEqual(branch.Image, partner.Image);
                Assert.AreEqual(double.Parse(branch.Discounts.First().Name.First().LocText), partner.Discount);
                Assert.AreEqual(partner.SelectDiscount, branch.Discounts.First().DiscountType);
                Assert.AreEqual(partner.Description_En, branch.Description.First(x => x.Lan == Languages.En).LocText);
                Assert.AreEqual(partner.Description_Ru, branch.Description.First(x => x.Lan == Languages.Ru).LocText);
                Assert.AreEqual(partner.Name_En, branch.Name.First(x => x.Lan == Languages.En).LocText);
                Assert.AreEqual(partner.Name_Ru, branch.Name.First(x => x.Lan == Languages.Ru).LocText);
                AssertCategories(partner.Categories, branch.CategoryIds);
                AssertWebAddresses(partner.WebAddresses?.ToList(), branch.WebAddresses?.ToList());
                AssertAddress(partner.Contacts?.ToList()[i], branch.Address?.ToList());
                AssertPhones(partner.Contacts?.ToList()[i], branch.Phones?.ToList());
                AssertCoordinates(partner.Contacts?.ToList()[i], branch.Location);
                AssertId(partner.Contacts?.ToList()[i], branch.Id);
            }

        }

        [TestMethod]
        public void MapToPartnerModelTestMethod()
        {
			// Arrange
			var branches = CreateBranches();

            // Act
            var resultPartnerModel = PartnerMapper.Map(branches, _categoryModels);

	        var firstBranch = branches.First();

			// Assert
			Assert.AreEqual(resultPartnerModel.Id, firstBranch.PartnerId);
            Assert.AreEqual(resultPartnerModel.Discount, double.Parse(firstBranch.Discounts.First().Name.First().LocText));
            Assert.AreEqual(resultPartnerModel.SelectDiscount, firstBranch.Discounts.First().DiscountType);
            Assert.AreEqual(resultPartnerModel.Comment, firstBranch.Comment);
            Assert.AreEqual(resultPartnerModel.Description_En, firstBranch.Description.First(x=>x.Lan == Languages.En).LocText);
            Assert.AreEqual(resultPartnerModel.Description_Ru, firstBranch.Description.First(x => x.Lan == Languages.Ru).LocText);
			Assert.AreEqual(resultPartnerModel.Image, firstBranch.Image);
            Assert.AreEqual(resultPartnerModel.Logo, firstBranch.Icon);
            Assert.AreEqual(resultPartnerModel.Name_En, firstBranch.Name.First(x => x.Lan == Languages.En).LocText);
			Assert.AreEqual(resultPartnerModel.Name_Ru, firstBranch.Name.First(x => x.Lan == Languages.Ru).LocText);
			AssertCategories(resultPartnerModel.Categories, firstBranch.CategoryIds);
            AssertWebAddresses(resultPartnerModel.WebAddresses?.ToList(), firstBranch.WebAddresses?.ToList());
            for(var i=0; i<branches.Count; i++)
            {
                var branch = branches[i];
                var contact = resultPartnerModel.Contacts?.ToList()[i];
                AssertAddress(contact, branch.Address?.ToList());
            }

            for (var i= 0; i < branches.Count; i++)
            {
                var branch = branches[i];
                var contact = resultPartnerModel.Contacts?.ToList()[i];
                AssertPhones(contact, branch.Phones?.ToList());
            }

            for (var i = 0; i < branches.Count; i++)
            {
                var branch = branches[i];
                var contact = resultPartnerModel.Contacts?.ToList()[i];
                AssertCoordinates(contact, branch.Location);
            }

            for (var i = 0; i < branches.Count; i++)
            {
                var branch = branches[i];
                var contact = resultPartnerModel.Contacts?.ToList()[i];
                AssertId(contact, branch.Id);
            }

        }

        private void AssertId(ContactModel contactModel, string id)
        {
            if (contactModel == null)
            {
                return;
            }
            Assert.AreEqual(contactModel.Id, id);
        }

        private void AssertCoordinates(ContactModel contactModel, GeoJsonPoint<GeoJson2DGeographicCoordinates> location)
        {
            if (contactModel == null)
            {
                return;
            }
            Assert.AreEqual(contactModel.Coordinates, location.Coordinates.Latitude.ToString(CultureInfo.InvariantCulture) + CoordSeparator +
                                                      location.Coordinates.Longitude.ToString(CultureInfo.InvariantCulture));

        }

        private void AssertAddress(ContactModel contactModel, List<LocalizableText> addresses)
        {
	        if (contactModel == null)
	        {
				return;
	        }

			Assert.IsTrue(addresses.Exists(x => x.Lan == Languages.Ru));
			Assert.AreEqual(addresses.First(x=>x.Lan == Languages.Ru).LocText, contactModel.Address_Ru);

		    Assert.IsTrue(addresses.Exists(x => x.Lan == Languages.En));
		    Assert.AreEqual(addresses.First(x => x.Lan == Languages.En).LocText, contactModel.Address_En);
		}

        private void AssertPhones(ContactModel contactModel, List<Phone> phones)
        {
            if (contactModel == null)
            {
                return;
            }

            Assert.AreEqual(contactModel.PhoneNumber1, phones[0].Number);
            Assert.AreEqual(contactModel.PhoneNumber2, phones[1].Number);

        }

		private void AssertCategories(IEnumerable<CategoryModel> categoryModels, IEnumerable<string> categories)
        {
            if (categoryModels == null)
            {
                return;
            }
	        foreach (var category in categoryModels)
	        {
				Assert.IsTrue(categories.Any(x=>x == category.Id));
			}
        }

        private void AssertWebAddresses(List<WebAddressModel> webAddresses, List<WebAddress> webAddressModels)
        {
            for (var i = 0; i < webAddresses.Count; i++)
            {
                Assert.AreEqual(webAddressModels[i].Id, webAddresses[i].Id);
                Assert.AreEqual(webAddressModels[i].Category, webAddresses[i].SocialNetwork);
                Assert.AreEqual(webAddressModels[i].Url, webAddresses[i].URL);
            }
        }
        private PartnerModel CreatePartnerModel()
        {
            var partner = new PartnerModel
            {
                Categories = new List<CategoryModel>()
                {
                    new CategoryModel()
                    {
                        Color = "#123",
                        Id = "CategoryIds1",
                        Name_Ru = "Category1Ru",
                        Name_En = "Category1En"
                    },
                    new CategoryModel()
                    {
                        Color = "#234",
                        Id = "CategoryIds2",
                        Name_Ru = "Category2Ru",
                        Name_En = "Category2En"
                    }
                },
                Id = "PartnerId",
                Image = "Image",
                Logo = "Icon",
                Name_Ru = "NameRu",
                Name_En = "NameEn",
                Discount = 12,
                SelectDiscount = "0",
                Description_Ru = "DescriptionRu",
                Description_En = "DescriptionEn",
                Comment = "Comment",
                WebAddresses = new List<WebAddressModel>()
                {
                    new WebAddressModel()
                    {
                        Id = "0",
                        SocialNetwork = "0",
                        URL = "Url"
                    },
                    new WebAddressModel()
                    {
                        Id = "1",
                        SocialNetwork = "1",
                        URL = "Url"
                    }
                },
                Contacts = new List<ContactModel>()
                {
                    new ContactModel()
                    {
                        Id = "Id",
                        Address_Ru = "AddressRu",
                        Address_En = "AddressEn",
                        PhoneNumber1 = "+375243546574",
                        PhoneNumber2 = "1234567890",
                        Coordinates = "23.6472367,19.3423478"
                    },
                    new ContactModel()
                    {
                    Id = "Id2",
                    Address_Ru = "AddressRu2",
                    Address_En = "AddressEn2",
                    PhoneNumber1 = "+125243546574",
                    PhoneNumber2 = "1234567890",
                    Coordinates = "23.6472367,19.3423478"
                }
                }
            };

            return partner;
        }
        private static List<Branch> CreateBranches()
        {
            var branches = new List<Branch>();
            var branch = new Branch
            {
                PartnerId = "PartnerId",
                Id = "Id",
                Icon = "Icon",
                Image = "Image",
                WebAddresses = new List<WebAddress>
                {
                    new WebAddress
                    {
                        Id = "0",
                        Category = "0",
                        Url = "Url"
                    },
                    new WebAddress
                    {
                        Id = "1",
                        Category = "1",
                        Url = "Url"
                    }
                },
                CategoryIds = new List<string>
                {
                    "CategoryIds1",
                    "CategoryIds2"
                },
                Comment = "Comment",
                Description = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "DescriptionRu"
                    },
                    new LocalizableText()
                    {
                        Lan = "EN",
                        LocText = "DescriptionEn"
                    }
                },
                Name = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "NameRu"
                    },
                    new LocalizableText
                    {
                        Lan = "EN",
                        LocText = "NameEn"
                    }
                },
                Address = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "AddressRu"
                    },
                    new LocalizableText
                    {
                        Lan = "EN",
                        LocText = "AddressEn"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone()
                    {
                        Number = "+375243546574"
                    },
                    new Phone()
                    {
                        Number = "1234567890"
                    }
                },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(19.3423478,23.6472367)),
                Discounts = new List<Discount>()
                {
                    new Discount()
                    {
                        DiscountType = "0",
                        Name = new List<LocalizableText>()
                        {
                            new LocalizableText()
                            {
                                Lan = "RU",
                                LocText = "12"
                            },
                            new LocalizableText()
                            {
                                Lan = "EN",
                                LocText = "12"
                            }
                        }
                    }
                }
            };
            branches.Add(branch);


            var TestBranch = new Branch
            {
                PartnerId = "PartnerId",
                Id = "Id",
                Icon = "Icon",
                Image = "Image",
                WebAddresses = new List<WebAddress>
                {
                    new WebAddress
                    {
                        Id = "0",
                        Category = "0",
                        Url = "Url"
                    },
                    new WebAddress
                    {
                        Id = "1",
                        Category = "1",
                        Url = "Url"
                    }
                },
                CategoryIds = new List<string>
                {
                    "CategoryIds1",
                    "CategoryIds2"
                },
                Comment = "Comment",
                Description = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "DescriptionRu"
                    },
                    new LocalizableText()
                    {
                        Lan = "EN",
                        LocText = "DescriptionEn"
                    }
                },
                Name = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "NameRu"
                    },
                    new LocalizableText
                    {
                        Lan = "EN",
                        LocText = "NameEn"
                    }
                },
                Address = new List<LocalizableText>
                {
                    new LocalizableText
                    {
                        Lan = "RU",
                        LocText = "TestAddressRu"
                    },
                    new LocalizableText
                    {
                        Lan = "EN",
                        LocText = "TestAddressEn"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone()
                    {
                        Number = "+37524354657412"
                    },
                    new Phone()
                    {
                        Number = "123456789012"
                    }
                },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(19.1223478, 23.1272367)),
                Discounts = new List<Discount>()
                {
                    new Discount()
                    {
                        DiscountType = "0",
                        Name = new List<LocalizableText>()
                        {
                            new LocalizableText()
                            {
                                Lan = "RU",
                                LocText = "12"
                            },
                            new LocalizableText()
                            {
                                Lan = "EN",
                                LocText = "12"
                            }
                        }
                    }
                }
            };
            branches.Add(TestBranch);
            return branches;
        }
    }
}
