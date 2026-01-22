using MediatR;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.PageTexts.Commands
{
    public class CreatePageTextCommand : IRequest<int>
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CreatePageTextHandler : IRequestHandler<CreatePageTextCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public CreatePageTextHandler(IApplicationDbContext context) => _context = context;

        public async Task<int> Handle(CreatePageTextCommand request, CancellationToken cancellationToken)
        {
            var entity = new PageText { Key = request.Key, Value = request.Value };
            _context.PageTexts.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.PageTextId;
        }
    }
}
