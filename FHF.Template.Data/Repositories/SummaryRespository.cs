using Microsoft.EntityFrameworkCore;
using FHF.Template.Data.Contexts;
using FHF.Template.Domain.Attributes;
using FHF.Template.Domain.Entities;
using FHF.Template.Domain.Interfaces.Repositories;

namespace FHF.Template.Data.Repositories;

[Repository]
public sealed class SummaryRespository(AppDbContext appDbContext)
    : BaseRepository<Summary>(appDbContext), ISummaryRespository
{
}