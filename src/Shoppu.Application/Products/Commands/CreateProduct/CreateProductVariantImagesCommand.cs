﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Products.Commands.CreateProduct
{
    public record CreateProductVariantImagesCommand(int VariantId, List<string> ImagePaths) : IRequest<bool>;

    public class CreateProductVariantImagesCommandHandler : IRequestHandler<CreateProductVariantImagesCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductVariantImagesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateProductVariantImagesCommand request, CancellationToken cancellationToken)
        {
            if (request.ImagePaths.Count() > 0)
            {
                var existingImages = await _context.ProductVariantImages
                    .Where(pvi => pvi.ProductVariantId == request.VariantId)
                    .ToListAsync();

                if (existingImages.Count > 0)
                {
                    _context.ProductVariantImages.RemoveRange(existingImages);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                foreach (var imagePath in request.ImagePaths)
                {
                    await _context.ProductVariantImages.AddAsync(new ProductVariantImage
                    {
                        ProductVariantId = request.VariantId,
                        ImageSource = imagePath,
                    });
                }
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }



}
