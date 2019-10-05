using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Services.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Setting> _sRepository;
        private readonly IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Field> _fieldRepository;

        public DataInitializer(
            IRepository<Role> roleRepository,
            IRepository<Setting> sRepository,
            IRepository<User> userRepository,
            UserManager<User> userManager,
            IRepository<Field> fieldRepository)
        {
            this._roleRepository = roleRepository ?? throw new System.ArgumentNullException(nameof(roleRepository));
            this._sRepository = sRepository ?? throw new System.ArgumentNullException(nameof(sRepository));
            this._userRepository = userRepository ?? throw new System.ArgumentNullException(nameof(userRepository));
            this._userManager = userManager;
            this._fieldRepository = fieldRepository ?? throw new System.ArgumentNullException(nameof(fieldRepository));
        }

        public void InitializeData()
        {
            //Role
            if (!_roleRepository.TableNoTracking.Any(p => p.Name == "Admin"))
            {
                _roleRepository.Add(new Role
                {
                    Name = "Admin",
                    Description = "Admin",
                    NormalizedName="ADMIN"
                });
               
            }
            if (!_roleRepository.TableNoTracking.Any(p => p.Name == "Personel"))
            {
                _roleRepository.Add(new Role
                {
                    Name = "Personel",
                    Description = "Personel",
                    NormalizedName="PERSONEL"
                });
            }
            if (!_roleRepository.TableNoTracking.Any(p => p.Name == "Teacher"))
            {
                _roleRepository.Add(new Role
                {
                    Name = "Teacher",
                    Description = "Teacher",
                    NormalizedName="TEACHER"
                });
            }
            if (!_roleRepository.TableNoTracking.Any(p => p.Name == "Student"))
            {
                _roleRepository.Add(new Role
                {
                    Name = "Student",
                    Description = "Student",
                    NormalizedName="STUDENT"
                });
            }

           

            //Field

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "عمومی"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "عمومی"
                });
            }

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "مهندسی کامپیوتر"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "مهندسی کامپیوتر"
                });
            }

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "علوم کامپیوتر"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "علوم کامپیوتر"
                });
            }

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "مهندسی صنایع"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "مهندسی صنایع"
                });
            }

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "مهندسی مکانیک"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "مهندسی مکانیک"
                });
            }

            if (!_fieldRepository.TableNoTracking.Any(p => p.Name == "مهندسی عمران"))
            {
                _fieldRepository.Add(new Field
                {
                    Name = "مهندسی عمران"
                });
            }

            if (_sRepository.GetById(1)==null)
            {
                var setting = new Setting
                {
                    Amount_Of_Punishment_For_Reserving_The_Book = 50,
                    Amount_Of_Punishment_For_Returning_The_Book = 200,
                    BorrowDay = 14,
                    ReservCount = 4,
                    BorrowCount = 2,
                    ReservDay = 2,
                    BDay4Reserve=2
                };
                _sRepository.Add(setting);
            }

            
        }
    }

}
