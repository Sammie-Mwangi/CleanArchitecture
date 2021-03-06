﻿using App.Application.EntitiesCommandsQueries.ProductCategories.Commands.CreateProductCategory;
using App.Application.EntitiesCommandsQueries.ProductCategories.Queries.GetProductCategory;
using App.Application.EntitiesCommandsQueries.ProductCategories.Queries.GetProductCategories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ProductCategoriesViewModel>> GetAll()
        {
            return Ok(await Mediator.Send(new GetProductCategoriesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryViewModel>> Get(string id)
        {

            Int64.TryParse(id, out long Id);
            return Ok(await Mediator.Send(new GetProductCategoryQuery { ID = Id }));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody]CreateProductCategoryCommand command)
        {
            return Created(CurrentUri, await Mediator.Send(command));
        }

        



    }
}