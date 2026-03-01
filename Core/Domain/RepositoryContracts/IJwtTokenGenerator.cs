using Core.Domain.IdentityEntities;
using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryContracts
{
    public interface IJwtTokenGenerator
    {
        TokenResult GenerateToken(ApplicationUser applicationUser);
    }
}
