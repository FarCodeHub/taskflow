using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaskFlow.Tasks.Domain.TaskItems;

public enum TaskItemStatus
{
    Todo = 1,
    InProgress = 2,
    Done = 3,
    Cancelled = 4
}
