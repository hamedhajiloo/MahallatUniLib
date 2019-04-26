using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _repository;

        public StudentService(IRepository<Student> repository)
        {
            _repository = repository;
        }

        public async Task<string> AddAsync(CancellationToken cancellationToken, StudentDto studentDto)
        {
            try
            {
                var model = await _repository.TableNoTracking.Where(c => c.Id == studentDto.Id || c.Code == studentDto.Code).SingleOrDefaultAsync(cancellationToken);
                if (model != null)
                    return "Exists";
                var student = studentDto.ToEntity();
                await _repository.AddAsync(student, cancellationToken);
                return "Ok";
            }
            catch
            {
                return "Bad";
            }
        }

        public async Task<List<StudentSelectDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<StudentSelectDto>().ToListAsync(cancellationToken);
        }

        public async Task<StudentSelectDto> GetByIdAsync(CancellationToken cancellationToken, string id)
        {
            var model= await _repository.TableNoTracking.Where(c => c.Id == id).ProjectTo<StudentSelectDto>().SingleOrDefaultAsync(cancellationToken);
            if (model == null) throw new BadRequestException("دانشجوی مورد نظر یافت نشد");
            return model;


        }

        public async Task<StudentSelectDto> GetByStudentNumberAsync(CancellationToken cancellationToken, string code)
        {
            var model= await _repository.TableNoTracking.Where(c => c.Code == code.Trim()).ProjectTo<StudentSelectDto>().SingleOrDefaultAsync(cancellationToken);
            if (model == null) throw new BadRequestException("دانشجوی مورد نظر یافت نشد");
            return model;

        }
    }
}