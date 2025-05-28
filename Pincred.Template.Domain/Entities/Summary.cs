using Pincred.Template.Domain.Entities.Base;

namespace Pincred.Template.Domain.Entities;

public class Summary : EntityBase
{
    public string Description { get; private set; }

    public Summary(string description)
    {
        Description = description;
    }
}