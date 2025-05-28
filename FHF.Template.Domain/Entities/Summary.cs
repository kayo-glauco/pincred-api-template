using FHF.Template.Domain.Entities.Base;

namespace FHF.Template.Domain.Entities;

public class Summary : EntityBase
{
    public string Description { get; private set; }

    public Summary(string description)
    {
        Description = description;
    }
}