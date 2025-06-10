using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators {
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>{
        public LoginUserCommandValidator() {
            RuleFor(u => u.Email).NotEmpty().WithMessage("O campo 'Email' não pode estar vazio.")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(u => u.Password).NotEmpty().WithMessage("O campo 'Password' não pode estar vazio.");
        }
    }
}
