﻿using Application.Features.Brands.Dtos;
using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<CreatedBrandDto>
    {
        public string Name
        {
            get; set;
        }
        public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandDto>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;
            private readonly BrandBusinessRules _brandBusinessRules;

            public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules _brandBusinessRules)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
                _brandBusinessRules = _brandBusinessRules;
            }

            public async Task<CreatedBrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {
                await _brandBusinessRules.BrandNameCanNotBeDuplicatedWhenInserted(request.Name);
                Brand mappedBrand = _mapper.Map<Brand>(request);
                Brand createdBrand = await _brandRepository.AddAsync(mappedBrand);
                CreatedBrandDto createdBrandDto = _mapper.Map<CreatedBrandDto>(createdBrand);
                return createdBrandDto;
            }
        }
    }
}
/*
 CQRS
- Klasik bir uygulamada bir manager açıp onun içine yerleştiriyoruz. Command veri tabanından yapılan Insert, Update, Delete işlemleri yapılırken Query ise select işlemleri yapılabilir.
- Kodu temiz ve sürdürülebilir olması çok önemli.

 */