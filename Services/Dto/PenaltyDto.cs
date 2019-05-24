using AutoMapper;
using Common;
using Common.Enums;
using Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Dto
{
    public class PenaltyDto : BaseDto<PenaltyDto, Penalty>
    {
        [DisplayName("نوع جریمه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public PenaltyType PenaltyType { get; set; }

        [DisplayName("مبلغ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public decimal Amount { get; set; }

        [DisplayName("کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserId { get; set; }

        [DisplayName("کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int BookId { get; set; }
    }

    public class PenaltySelectDto : BaseDto<PenaltySelectDto, Penalty>
    {
        [DisplayName("نوع جریمه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public PenaltyType PenaltyType { get; set; }

        [DisplayName("مبلغ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public decimal Amount { get; set; }

        [DisplayName("شناسه کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserId { get; set; }

        [DisplayName("نام و نام خانوادگی کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserFullName { get; set; }

        [DisplayName("شناسه کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int BookId { get; set; }

        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string BookName { get; set; }

        public override void CustomMappings(IMappingExpression<Penalty, PenaltySelectDto> mapping)
        {
            mapping.ForMember(des => des.UserFullName, opt => opt.MapFrom(src => src.User.FullName));
            mapping.ForMember(des => des.BookName, opt => opt.MapFrom(src => src.BookList.Name));
        }
    }
}