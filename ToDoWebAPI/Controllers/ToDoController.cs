using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        ToDoRepository _repository = new ToDoRepository();

        public ToDoController()
        {

        }

        // GET: api/<ToDoController>
        [HttpGet]
        public IEnumerable<ToDoDto> Get()
        {
            return _repository.GetAll();
        }

        // POST api/<ToDoController>
        [HttpPost]
        public int Post([FromBody] ToDoDto toDoDto)
        {
            return _repository.Create(toDoDto);
        }

        // PUT api/<ToDoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ToDoDto toDoDto)
        {
            _repository.Update(id, toDoDto);
        }
    }
}
    