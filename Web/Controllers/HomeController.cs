using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Enums;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Dto;
using Web.Model;
using WebFramework.Api;
using WebFramework.Filters;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRepository<Field> _fieldRepository;
        private readonly IRepository<Book> _bookRepository;

        public HomeController(IBookService bookService,IRepository<Field> fieldRepository,IRepository<Book> bookRepository)
        {
            _bookService = bookService;
            _fieldRepository = fieldRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var pagable = new Pagable
            {
                Desc = true,
                Page = 1,
                PageSize = 10
            };

            var vm = new HomeIndexVM();

            vm.Fields = await _fieldRepository.TableNoTracking.ToListAsync(cancellationToken);
            vm.General = await _bookRepository.TableNoTracking.Where(c => c.FieldId==1 &&c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.ComputerBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 2 && c.BookIsDeleted == false).Take(10).ToListAsync(cancellationToken);
            vm.UlomBooks = await _bookRepository.TableNoTracking.Where(c =>c.FieldId==3 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.SanayeBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 4 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.MechanickBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 5 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
            vm.OmranBooks = await _bookRepository.TableNoTracking.Where(c => c.FieldId == 6 && c.BookIsDeleted==false).Take(10).ToListAsync(cancellationToken);
          
            return View(vm);
        }
       
    }
}