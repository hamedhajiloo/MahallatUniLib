using AutoMapper.QueryableExtensions;
using Common;
using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public StudentService(IRepository<Student> repository,UserManager<User> userManager)
        {
            _repository = repository;
            this._userManager = userManager;
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

            if (model.FieldId == 1)
                model.FieldStatus = FieldStatus.N;

            if (model.FieldId == 2)
                model.FieldStatus = FieldStatus.C;


            if (model.FieldId == 3)
                model.FieldStatus = FieldStatus.PC;

            if (model.FieldId ==4)
                model.FieldStatus = FieldStatus.S;

            if (model.FieldId == 5)
                model.FieldStatus = FieldStatus.M;

            if (model.FieldId == 6)
                model.FieldStatus = FieldStatus.O;

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

        public async Task<List<StudentSelectDto>> GetAllAsync(CancellationToken cancellationToken, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return await _repository.TableNoTracking.Include(c => c.User).Include(c => c.Field).ProjectTo<StudentSelectDto>().ToListAsync(cancellationToken);
            }
            return await _repository.TableNoTracking.Include(c=>c.User).Include(c=>c.Field)
                .Where(c=>
                c.User.FullName.Contains(search)||
                c.User.UserName.Contains(search)||
                c.Field.Name.Contains(search))
                .ProjectTo<StudentSelectDto>().ToListAsync(cancellationToken);
        }

        public async Task EditAsync(StudentSelectDto studentDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(studentDto.UserId);
            if (user==null)
                throw new BadRequestException("کاربر یافت نشد");

            user.FullName = studentDto.StudentFullName;
            user.UserName = studentDto.UserName;
            await _userManager.UpdateAsync(user);


            var student = await _repository.Table.Where(c => c.Id == studentDto.Id).SingleOrDefaultAsync(cancellationToken);
            if (student == null)
                throw new BadRequestException("کاربر یافت نشد");

            student.StudentStatus = studentDto.StudentStatusEnum;
            student.FieldId = studentDto.FieldId;
            await _repository.UpdateAsync(student, cancellationToken);
        }
    }
}