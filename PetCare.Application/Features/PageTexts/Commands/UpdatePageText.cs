using MediatR;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PageTexts.Commands
{
    public class UpdatePageTextCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class UpdatePageTextHandler : IRequestHandler<UpdatePageTextCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public UpdatePageTextHandler(IApplicationDbContext context) => _context = context;

        public async Task<int> Handle(UpdatePageTextCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PageTexts.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null) throw new NotFoundException("PageText", request.Id);

            entity.Key = request.Key;
            entity.Value = request.Value;

            await _context.SaveChangesAsync(cancellationToken);
            return entity.PageTextId;
        }
    }
}
