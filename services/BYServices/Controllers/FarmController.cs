using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{
    public class FarmController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetPetInfo(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);

                int petId = obj["petId"].Value<int>();
                if (petId <= 0)
                {
                    throw BuildHttpResponseException("Invalid pet ID", "ERR_PET_ID");
                }
                Animal pet = db.Animals.SingleOrDefault(x => x.Id == petId);
                return new AnimalModel(pet);
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetPetsList(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);

                List<Animal> tempPets = db.Animals.ToList();
                List<AnimalModel> pets = new List<AnimalModel>();
                foreach (Animal pet in tempPets)
                {
                    pets.Add(new AnimalModel(pet));
                }
                return pets;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetFarmByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                List<FarmModel> userPets = db.UserFarms.Where(x => x.UserId == currentUser.Id).ToList().Select(x => new FarmModel(x.PetId, x.Animal.Breed, x.PetCount, x.EggsInStock, x.EggsHatching)).ToList();

                return userPets;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage UpdateFarmPetHistoryByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                int petId = obj["petId"].Value<int>();
                Animal pet = ValidatePetId(petId);

                string action = obj["action"].Value<string>();
                if (action != "ADDP" && action != "SELLP" && action != "LOSEP")
                {
                    throw BuildHttpResponseException("Invalid Pet Farm Action", "ERR_PET_ACT");
                }

                int count = obj["count"].Value<int>();
                if (count < 0)
                {
                    throw BuildHttpResponseException("Invalid Pet Count", "ERR_PET_PRC");
                }

                UserFarm uf = db.UserFarms.SingleOrDefault(x => x.PetId == petId && x.UserId == currentUser.Id);

                double price = obj["price"].Value<double>();

                switch (action)
                {
                    case "ADDP":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Pet Price", "ERR_PET_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    UserFarm tempUF = new UserFarm()
                                    {
                                        UserId = currentUser.Id,
                                        PetId = petId,
                                        EggsInStock = 0,
                                        EggsHatching = 0,
                                        PetCount = count
                                    };
                                    db.UserFarms.Add(tempUF);
                                }
                                else
                                {
                                    uf.PetCount += count;
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = count,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = 0,
                                    EggsStocked = 0,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "SELLP":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Pet Price", "ERR_PET_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    throw BuildHttpResponseException("Can not sell pet you dont have", "ERR_INV_PET");
                                }
                                else
                                {
                                    uf.PetCount -= count;
                                    if (uf.PetCount < 0)
                                    {
                                        throw BuildHttpResponseException("Can not sell more pets that you have", "ERR_INV_PET");
                                    }
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = count,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = 0,
                                    EggsStocked = 0,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "LOSEP":
                        {
                            if (uf == null)
                            {
                                throw BuildHttpResponseException("Can not lose pet you dont have", "ERR_INV_PET");
                            }
                            else
                            {
                                uf.PetCount -= count;
                                if (uf.PetCount < 0)
                                {
                                    throw BuildHttpResponseException("Can not lose more pets that you have", "ERR_INV_PET");
                                }
                            }
                            UserFarmHistory tempFarmHistory = new UserFarmHistory()
                            {
                                PetCount = count,
                                PetId = petId,
                                PetStatus = action,
                                EggsHatching = 0,
                                EggsStocked = 0,
                                UserId = currentUser.Id,
                                ActionDate = DateTime.Now,
                                PetPrice = 0
                            };
                            db.UserFarmHistories.Add(tempFarmHistory);
                            db.SaveChanges();
                            break;
                        }
                    default:
                        break;
                }
                return "Success";
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage UpdateFarmHatchingHistoryByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                int petId = obj["petId"].Value<int>();
                Animal pet = ValidatePetId(petId);

                string action = obj["action"].Value<string>();
                if (action != "ADDH" && action != "SELLH" && action != "LOSEH")
                {
                    throw BuildHttpResponseException("Invalid Hatching Egg Action", "ERR_HTC_ACT");
                }

                int count = obj["count"].Value<int>();
                if (count < 0)
                {
                    throw BuildHttpResponseException("Invalid Hatching Egg Count", "ERR_HTC_CNT");
                }

                UserFarm uf = db.UserFarms.SingleOrDefault(x => x.PetId == petId && x.UserId == currentUser.Id);

                double price = obj["price"].Value<double>();

                switch (action)
                {
                    case "ADDH":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Hatching egg Price", "ERR_HTC_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    UserFarm tempUF = new UserFarm()
                                    {
                                        UserId = currentUser.Id,
                                        PetId = petId,
                                        EggsInStock = 0,
                                        EggsHatching = count,
                                        PetCount = 0
                                    };
                                    db.UserFarms.Add(tempUF);
                                }
                                else
                                {
                                    uf.EggsHatching += count;
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = 0,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = count,
                                    EggsStocked = 0,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "SELLH":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Hatching Egg Price", "ERR_HTC_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    throw BuildHttpResponseException("Can not sell eggs of pet you dont have", "ERR_HTC_INV");
                                }
                                else
                                {
                                    uf.EggsHatching -= count;
                                    if (uf.EggsHatching < 0)
                                    {
                                        throw BuildHttpResponseException("Can not sell more hatching eggs that you have", "ERR_HTC_CNT");
                                    }
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = 0,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = count,
                                    EggsStocked = 0,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "LOSEH":
                        {
                            if (uf == null)
                            {
                                throw BuildHttpResponseException("Can not lose hatching eggs of pet you dont have", "ERR_HTC_PET");
                            }
                            else
                            {
                                uf.EggsHatching -= count;
                                if (uf.PetCount < 0)
                                {
                                    throw BuildHttpResponseException("Can not lose more hatchings eggs that you have", "ERR_HTC_CNT");
                                }
                            }
                            UserFarmHistory tempFarmHistory = new UserFarmHistory()
                            {
                                PetCount = 0,
                                PetId = petId,
                                PetStatus = action,
                                EggsHatching = count,
                                EggsStocked = 0,
                                UserId = currentUser.Id,
                                ActionDate = DateTime.Now,
                                PetPrice = 0
                            };
                            db.UserFarmHistories.Add(tempFarmHistory);
                            db.SaveChanges();
                            break;
                        }
                    default:
                        break;
                }
                return "Success";
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage UpdateFarmEggsHistoryByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                int petId = obj["petId"].Value<int>();
                Animal pet = ValidatePetId(petId);

                string action = obj["action"].Value<string>();
                if (action != "ADDE" && action != "SELLE" && action != "LOSEE")
                {
                    throw BuildHttpResponseException("Invalid Eggs Action", "ERR_EGG_ACT");
                }

                int count = obj["count"].Value<int>();
                if (count < 0)
                {
                    throw BuildHttpResponseException("Invalid Eggs Count", "ERR_EGG_CNT");
                }

                UserFarm uf = db.UserFarms.SingleOrDefault(x => x.PetId == petId && x.UserId == currentUser.Id);

                double price = obj["price"].Value<double>();

                switch (action)
                {
                    case "ADDE":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Eggs Price", "ERR_EGG_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    UserFarm tempUF = new UserFarm()
                                    {
                                        UserId = currentUser.Id,
                                        PetId = petId,
                                        EggsInStock = count,
                                        EggsHatching = 0,
                                        PetCount = 0
                                    };
                                    db.UserFarms.Add(tempUF);
                                }
                                else
                                {
                                    uf.EggsInStock += count;
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = 0,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = 0,
                                    EggsStocked = count,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "SELLE":
                        {
                            if (price < 0)
                            {
                                throw BuildHttpResponseException("Invalid Eggs Price", "ERR_EGG_PRC");
                            }
                            else
                            {
                                if (uf == null)
                                {
                                    throw BuildHttpResponseException("Can not sell eggs of pet you dont have", "ERR_EGG_INV");
                                }
                                else
                                {
                                    uf.EggsInStock -= count;
                                    if (uf.EggsInStock < 0)
                                    {
                                        throw BuildHttpResponseException("Can not sell more eggs than you have", "ERR_HTC_CNT");
                                    }
                                }
                                UserFarmHistory tempFarmHistory = new UserFarmHistory()
                                {
                                    PetCount = 0,
                                    PetId = petId,
                                    PetStatus = action,
                                    EggsHatching = 0,
                                    EggsStocked = count,
                                    UserId = currentUser.Id,
                                    ActionDate = DateTime.Now,
                                    PetPrice = price
                                };
                                db.UserFarmHistories.Add(tempFarmHistory);
                                db.SaveChanges();
                            }
                            break;
                        }
                    case "LOSEE":
                        {
                            if (uf == null)
                            {
                                throw BuildHttpResponseException("Can not lose eggs of pet you dont have", "ERR_HTC_PET");
                            }
                            else
                            {
                                uf.EggsInStock -= count;
                                if (uf.EggsInStock < 0)
                                {
                                    throw BuildHttpResponseException("Can not lose more hatchings eggs that you have", "ERR_HTC_CNT");
                                }
                            }
                            UserFarmHistory tempFarmHistory = new UserFarmHistory()
                            {
                                PetCount = 0,
                                PetId = petId,
                                PetStatus = action,
                                EggsHatching = 0,
                                EggsStocked = count,
                                UserId = currentUser.Id,
                                ActionDate = DateTime.Now,
                                PetPrice = 0
                            };
                            db.UserFarmHistories.Add(tempFarmHistory);
                            db.SaveChanges();
                            break;
                        }
                    default:
                        break;
                }
                return "Success";
            });
            return msg;
        }
        
        private Animal ValidatePetId(int petId)
        {
            Animal pet = db.Animals.SingleOrDefault(x => x.Id == petId);
            if (pet == null)
            {
                throw BuildHttpResponseException("Invalid Pet Id", "ERR_PET_ID");
            }
            return pet;
        }
    }
}
