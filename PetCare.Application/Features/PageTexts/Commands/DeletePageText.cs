using MediatR;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.PageTexts.Commands
{
    public record DeletePageTextCommand(int Id) : IRequest<int>;

    public class DeletePageTextHandler : IRequestHandler<DeletePageTextCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public DeletePageTextHandler(IApplicationDbContext context) => _context = context;

        public async Task<int> Handle(DeletePageTextCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PageTexts.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("TextPage", request.Id);
            }

            _context.PageTexts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.PageTextId;
        }
    }
}
