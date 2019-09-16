using Services.Dto;
using Services.Enum.ServicesResult.Student;
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
        Task<List<StudentSelectDto>> GetAllAsync(CancellationToken cancellationToken,string search);
        Task<AddAsyncStatus> AddAsync(CancellationToken cancellationToken, StudentDto studentDto);
        Task<StudentSelectDto> GetByStudentNumberAsync(CancellationToken cancellationToken, string code);
        Task<StudentSelectDto> GetByIdAsync(CancellationToken cancellationToken, string id);
        Task<DeleteAsyncStatus> DeleteAsync(string studentId, CancellationToken cancellationToken);
        Task<ServiceResult<StudentSelectDto>> FindByIdAsync(string id, CancellationToken cancellationToken);
        Task EditAsync(StudentSelectDto studentDto, CancellationToken cancellationToken);
    }
}
