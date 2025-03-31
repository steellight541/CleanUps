using CleanUps.Shared.DTOs.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUps.Shared.DTOs
{
    public record RoleDTO(int RoleId, string RoleName) : RecordDTO;
}
