﻿using BuildingBlock.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Product.CreatProduct;
using Marten;

namespace Catalog.API.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandHandler (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);
            var product = await session.LoadAsync<Models.Product>(command.Id, cancellationToken);
            if(product == null)
            {
                throw  new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile   = command.ImageFile;
            product.Price   = command.Price;
            session.Update(product);
            await session.SaveChangesAsync();

            return new UpdateProductResult(true);

        }
    }
}