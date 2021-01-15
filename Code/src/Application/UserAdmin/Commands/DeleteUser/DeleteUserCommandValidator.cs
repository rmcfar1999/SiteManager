using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SiteManager.V4.Application.UserAdmin.Commands
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandValidator(IApplicationDbContext context)
        {
            //_context = context;

            //RuleFor(v => v.SurveyId)
            //    .MustAsync(SurveyExists)
            //    .WithMessage("The survey specified does not exist.");
        }

        //public async Task<bool> SurveyExists(int surveyId, CancellationToken cancellationToken)
        //{
        //    return await _context.Surveys.AnyAsync(x => x.SurveyId == surveyId);
                
        //}
    }
}
