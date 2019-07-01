using Data.Repositories;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.DataInitializer
{
    public class FieldDataInitializer : IDataInitializer
    {
        private readonly IRepository<Field> repository;

        public FieldDataInitializer(IRepository<Field> repository)
        {
            this.repository = repository;
        }

        public void InitializeData()
        {
            if (!repository.TableNoTracking.Any(p => p.Name=="مهندسی کامپیوتر"))
            {
                repository.Add(new Field
                {
                    Name= "مهندسی کامپیوتر"
                });
            }

            if (!repository.TableNoTracking.Any(p => p.Name == "علوم کامپیوتر"))
            {
                repository.Add(new Field
                {
                    Name = "علوم کامپیوتر"
                });
            }

            if (!repository.TableNoTracking.Any(p => p.Name == "مهندسی صنایع"))
            {
                repository.Add(new Field
                {
                    Name = "مهندسی صنایع"
                });
            }

            if (!repository.TableNoTracking.Any(p => p.Name == "مهندسی مکانیک"))
            {
                repository.Add(new Field
                {
                    Name = "مهندسی مکانیک"
                });
            }

            if (!repository.TableNoTracking.Any(p => p.Name == "مهندسی عمران"))
            {
                repository.Add(new Field
                {
                    Name = "مهندسی عمران"
                });
            }

            if (!repository.TableNoTracking.Any(p => p.Name == "معماری"))
            {
                repository.Add(new Field
                {
                    Name = "معماری"
                });
            }


        }
    }
}
