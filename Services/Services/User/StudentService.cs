using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using Services.Enum.ServicesResult.Student;
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

        public async Task<AddAsyncStatus> AddAsync(CancellationToken cancellationToken, StudentDto studentDto)
        {
            try
            {
                var model = await _repository.TableNoTracking.Where(c => c.Id == studentDto.Id || c.User.UserName == studentDto.UserName).SingleOrDefaultAsync(cancellationToken);
                if (model != null)
                    return AddAsyncStatus.Exists;
                var student = studentDto.ToEntity();
                await _repository.AddAsync(student, cancellationToken);
                return AddAsyncStatus.Ok;
            }
            catch (Exception)
            {
                return AddAsyncStatus.Bad;
            }
        }

        public async Task<List<StudentSelectDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<StudentSelectDto>().ToListAsync(cancellationToken);
        }

        public async Task<StudentSelectDto> GetByIdAsync(CancellationToken cancellationToken, string id)
        {
            var model = await _repository.TableNoTracking.Where(c => c.Id == id).ProjectTo<StudentSelectDto>().SingleOrDefaultAsync(cancellationToken);
            if (model == null) throw new BadRequestException("دانشجوی مورد نظر یافت نشد");
            return model;


        }

        public async Task<StudentSelectDto> GetByStudentNumberAsync(CancellationToken cancellationToken, string code)
        {
            var model = await _repository.TableNoTracking.Where(c => c.User.UserName == code.Trim()).ProjectTo<StudentSelectDto>().SingleOrDefaultAsync(cancellationToken);
            return model;
        }

        public async Task<DeleteAsyncStatus> DeleteAsync(string studentId, CancellationToken cancellationToken)
        {
            try
            {
                var model = await _repository.Table.SingleOrDefaultAsync(c => c.Id == studentId, cancellationToken);
                if (model == null)
                    return DeleteAsyncStatus.NonExists;
                await _repository.DeleteAsync(model, cancellationToken);
                return DeleteAsyncStatus.Ok;
            }
            catch
            {
                return DeleteAsyncStatus.Bad;
            }
        }

        public async Task<ServiceResult<StudentSelectDto>> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            var model = await _repository.TableNoTracking.Where(c => c.Id == id).ProjectTo<StudentSelectDto>().FirstOrDefaultAsync(cancellationToken);
            if (model == null)
                return new ServiceResult<StudentSelectDto>(false,ApiResultStatusCode.NotFound,null, "دانشجو یافت نشد");

            return new ServiceResult<StudentSelectDto>(true,ApiResultStatusCode.Success,model);
        }
    }
}