using NinaApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinaApp.Core.ServiceContracts
{
  public interface IJwtTokenGenerator
  {
    string GenerateToken(User user);
  }
}
