using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.DataInitializer
{
    public class TestDataInitializer : IDataInitializer
    {
        private readonly IRepository<User> repository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Student> studentRepository;

        public TestDataInitializer(IRepository<User> repository,UserManager<User> userManager,IRepository<Student> studentRepository)
        {
            this.repository = repository;
            this._userManager = userManager;
            this.studentRepository = studentRepository;
        }

        public void InitializeData()
        {
            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList=new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=2}
            //        },
            //        ImageUrl= "/site/img/Test/4.png",
            //        AuthorName="Test",
            //        BookStatus=BookStatus.Free,
            //        Edition=1,
            //        CourseType=Common.CourseType.Special,
            //        Language=Common.Language.Persion,
            //        Publisher="Test",
            //        PublishYear=1380
            //    });

            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList = new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=3}
            //        },
            //        ImageUrl = "/site/img/Test/4.png",
            //        AuthorName = "Test",
            //        BookStatus = BookStatus.Free,
            //        Edition = 1,
            //        CourseType = Common.CourseType.Special,
            //        Language = Common.Language.Persion,
            //        Publisher = "Test",
            //        PublishYear = 1380
            //    });

            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList = new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=4}
            //        },
            //        ImageUrl = "/site/img/Test/4.png",
            //        AuthorName = "Test",
            //        BookStatus = BookStatus.Free,
            //        Edition = 1,
            //        CourseType = Common.CourseType.Special,
            //        Language = Common.Language.Persion,
            //        Publisher = "Test",
            //        PublishYear = 1380
            //    });

            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList = new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=4}
            //        },
            //        ImageUrl = "/site/img/Test/4.png",
            //        AuthorName = "Test",
            //        BookStatus = BookStatus.Free,
            //        Edition = 1,
            //        CourseType = Common.CourseType.Special,
            //        Language = Common.Language.Persion,
            //        Publisher = "Test",
            //        PublishYear = 1380
            //    });

            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList = new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=5}
            //        },
            //        ImageUrl = "/site/img/Test/4.png",
            //        AuthorName = "Test",
            //        BookStatus = BookStatus.Free,
            //        Edition = 1,
            //        CourseType = Common.CourseType.Special,
            //        Language = Common.Language.Persion,
            //        Publisher = "Test",
            //        PublishYear = 1380
            //    });

            //}

            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new BookList
            //    {
            //        Name = $"Book {i}",
            //        FieldBookList = new List<FieldBookList>
            //        {
            //            new FieldBookList{FieldId=6}
            //        },
            //        ImageUrl = "/site/img/Test/4.png",
            //        AuthorName = "Test",
            //        BookStatus = BookStatus.Free,
            //        Edition = 1,
            //        CourseType = Common.CourseType.Special,
            //        Language = Common.Language.Persion,
            //        Publisher = "Test",
            //        PublishYear = 1380
            //    });

            //}


            //for (int i = 0; i < 20; i++)
            //{
            //    repository.Add(new User
            //    {
            //        FullName = $"user {i}",
            //        UserName = $"username {i}"
            //    });
            //}

            //var m = repository.Table.Where(c => c.UserName.StartsWith("username")).ToList();

            //foreach (var item in m)
            //{
              
            //    _userManager.AddToRoleAsync(item, "Student");
            //}
            //foreach (var item in m)
            //{
            //    studentRepository.Add(new Student
            //    {
            //        UserId = item.Id,
            //        FieldId = 1

            //    });
            //}
        }
    }
}
