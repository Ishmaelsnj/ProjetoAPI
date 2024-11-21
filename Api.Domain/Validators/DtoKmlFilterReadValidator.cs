using Api.Domain.DTO;
using FluentValidation;


namespace Api.Domain.Validators;

public class DtoKmlFilterReadValidator : AbstractValidator<DtoKmlFilterRead>
{
    public DtoKmlFilterReadValidator(List<DtoKmlFilterWrite> filters)
    {
        RuleFor(x => x)
            .Must(x => filters.Any(f => f.Cliente == x.Cliente
                && f.Situacao == x.Situacao
                && f.Bairro == x.Bairro))
            .WithMessage("A combinação de cliente, situação e bairro não foram encontradas.");

        RuleFor(x => x.Referencia)
            .Must(referencia => referencia.Length >= 3)
            .WithMessage("O campo 'Referência' deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.RuaCruzamento)
            .Must(ruaCruzamento => ruaCruzamento.Length >= 3)
            .WithMessage("O campo 'Rua/Cruzamento' deve ter pelo menos 3 caracteres.");
    }
}
