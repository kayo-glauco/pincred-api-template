using Microsoft.EntityFrameworkCore;
using Pincred.Template.Data.Contexts;
using Pincred.Template.Domain.Attributes;
using Pincred.Template.Domain.Entities;
using Pincred.Template.Domain.Interfaces.Repositories;

namespace Pincred.Template.Data.Repositories;

[Repository]
public sealed class SummaryRespository(AppDbContext appDbContext)
    : BaseRepository<Summary>(appDbContext), ISummaryRespository
{
}