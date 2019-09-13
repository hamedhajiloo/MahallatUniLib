using Data.Repositories;
using Entities;
using System.Linq;

namespace Services.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Field> _fieldRepository;

        public DataInitializer(IRepository<Role> roleRepository, IRepository<Field> fieldRepository)
        {
            this._roleRepository = roleRepository;
            this._fieldRepository = fieldRepository;
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

        }
    }

}
