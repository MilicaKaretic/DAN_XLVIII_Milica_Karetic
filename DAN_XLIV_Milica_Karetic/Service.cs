using DAN_XLIV_Milica_Karetic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XLIV_Milica_Karetic
{
    public class Service
    {
        public static List<tblUser> loggedUser = new List<tblUser>();
        public static List<tblItem> items = new List<tblItem>();
        public static tblUser currentUser = new tblUser();
        public static tblOrder currentOrder = new tblOrder();

        /// <summary>
        /// get all users from database
        /// </summary>
        /// <returns></returns>
        public List<tblUser> GetAllUsers()
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    List<tblUser> users = new List<tblUser>();
                    users = context.tblUsers.ToList();
                    return users;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// get all items from database
        /// </summary>
        /// <returns></returns>
        public List<tblItem> GetAllItems()
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    List<tblItem> items = new List<tblItem>();
                    items = context.tblItems.ToList();
                    return items;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// delete order
        /// </summary>
        /// <param name="orderID">order id</param>
        public void DeleteOrder(int orderID)
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    tblOrder orderToDelete = (from r in context.tblOrders where r.OrderID == orderID select r).First();
                    context.tblOrders.Remove(orderToDelete);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        /// <summary>
        /// deny order
        /// </summary>
        /// <param name="orderID">order id</param>
        public void DenyOrder(int orderID)
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    tblOrder orderToDeny = (from r in context.tblOrders where r.OrderID == orderID select r).First();
                    orderToDeny.OrderStatus = "denied";
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        /// <summary>
        /// get all orders
        /// </summary>
        /// <returns></returns>
        public List<tblOrder> GetAllOrders()
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    List<tblOrder> orders = new List<tblOrder>();
                    orders = context.tblOrders.ToList();
                    return orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// approve order
        /// </summary>
        /// <param name="orderID"></param>
        public void ApproveOrder(int orderID)
        {
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    tblOrder orderToDeny = (from r in context.tblOrders where r.OrderID == orderID select r).First();
                    orderToDeny.OrderStatus = "approved";
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        /// <summary>
        /// chech user order status 
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns>pending or not</returns>
        public bool CheckOrderStatus(int userID)
        {
            int a = 0;
            try
            {
                using (OrderDBEntities1 context = new OrderDBEntities1())
                {
                    List<tblOrder> orders = new List<tblOrder>();
                    orders = context.tblOrders.ToList();

                    List<tblOrder> userOrders = orders.Where(u => u.UserID == userID).ToList();

                    for (int i = 0; i < userOrders.Count; i++)
                    {
                        if (userOrders[i].OrderStatus == "pending")
                            a++;
                    }
                    
                }
                if (a > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
    }
}
