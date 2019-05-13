﻿using Services.Dto;
using Services.Enum.Student;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IStudentService
    {
        Task<List<StudentSelectDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<AddAsyncStatus> AddAsync(CancellationToken cancellationToken, StudentDto studentDto);
        Task<StudentSelectDto> GetByStudentNumberAsync(CancellationToken cancellationToken, string code);
        Task<StudentSelectDto> GetByIdAsync(CancellationToken cancellationToken, string id);
    }
}