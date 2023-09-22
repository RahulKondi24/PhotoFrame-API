using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RKdigitalsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RKdigitalsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RKdigitalsDBContext _db;
        public ProductController()
        {
            _db = new RKdigitalsDBContext();
        }
        [HttpGet("FetchProducts")]
        public async Task<List<Product>> GetProduct()
        {
            return await _db.Products.ToListAsync();
        }
        [HttpGet("FetchProductById/{ID}")]
        public async Task<Product> GetProduct(int ID)
        {
            return await _db.Products.FirstOrDefaultAsync(x => x.Id == ID);
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> InsertProduct(Product product)
        {
            if (product != null)
            {
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    Message="Product Add Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Product Not Add Successfully"
                });
            }    
        }
        [HttpPut("EditProduct")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            Product p = new Product();
            p.Id = product.Id;
            p.Productname = product.Productname;
            p.Productdescription = product.Productdescription;
            p.Productimage = product.Productimage;
            p.Productprice = product.Productprice;
            if (p != null)
            {
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Product Updated Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Product Not Updated Successfully"
                });
            }
        }
        [HttpDelete("RemoveProductById/{ID}")]
        public async Task<IActionResult> DeleteProduct(int ID)
        {
            var data = await _db.Products.Where(x => x.Id == ID).FirstOrDefaultAsync();
            _db.Products.Remove(data);
            await  _db.SaveChangesAsync();
            return Ok(new
            {
                Message = "Product Removed Successfully"
            });
        }
        [HttpGet("Cart/{username}")]
        public async Task<Object> Cart(string username)
        {
            var innerJoin =await (from c in _db.Carts
                             join p in _db.Products
                                          on c.ProductId equals p.Id
                             join u in _db.Users
                                          on c.UserId equals u.Id
                             select new
                             {
                                 CartID = c.Id,
                                 Quntity = c.Quntity,
                                 PRODUCTNAME = p.Productname,
                                 PRODUCTIMAGE = p.Productimage,
                                 PRODUCTDESCRIPTION = p.Productdescription,
                                 PRODUCTPRICE = p.Productprice,
                                 USERNAME = u.Username,
                                 UserID = u.Id
                             }).Where(x => x.USERNAME == username).ToListAsync();
            return innerJoin;
        }
        [HttpPost("AddProducttoCart")]
        public async Task<IActionResult> AddProducttoCart(Cart cart)
        {
            if (cart != null)
            {
                if (cart.UserId != null)
                {
                    await _db.Carts.AddAsync(cart);
                    await _db.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = "Product Add the cart Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Please Login First"
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Product Not Add the cart Successfully"
                });
            }
        }
        [HttpDelete("RemoveProducttoCart/{Id}")]
        public async Task<IActionResult> RemoveProducttoCart(int Id)
        {
            if (Id != null)
            {
                var data = await _db.Carts.Where(x => x.Id == Id).FirstOrDefaultAsync();
                _db.Carts.Remove(data);
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Product is Deleted to the cart Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Product is Not Deleted the cart Successfully"
                });
            }
        }
        [HttpDelete("RemoveProductAlltoCart/{Id}")]
        public async Task<IActionResult> RemoveProductAlltoCart(int Id)
        {
            if (Id != null)
            {
                var data = await _db.Carts.Where(x => x.UserId == Id).ToListAsync();
                for (int i = 0; i < data.Count; i++)
                {
                    _db.Carts.Remove(data[i]);
                    await _db.SaveChangesAsync();
                }
                return Ok(new
                {
                    Message = "All Products is Deleted to the cart Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "All Products is Not Deleted the cart Successfully"
                });
            }
        }
    }
}
