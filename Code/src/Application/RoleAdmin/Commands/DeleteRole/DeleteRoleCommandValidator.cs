using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteRoleCommandValidator(IApplicationDbContext context)
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
