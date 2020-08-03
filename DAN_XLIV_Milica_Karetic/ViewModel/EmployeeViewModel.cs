using DAN_XLIV_Milica_Karetic.Commands;
using DAN_XLIV_Milica_Karetic.Model;
using DAN_XLIV_Milica_Karetic.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DAN_XLIV_Milica_Karetic.ViewModel
{
    class EmployeeViewModel : BaseViewModel
    {
        Employee view;
        Service service = new Service();

        #region Property

        private tblOrder order;
        public tblOrder Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        private List<tblOrder> orderList;
        public List<tblOrder> OrderList
        {
            get
            {
                return orderList;
            }
            set
            {
                orderList = value;
                OnPropertyChanged("OrderList");
            }
        }

        private Visibility viewOrder = Visibility.Visible;
        public Visibility ViewOrder
        {
            get
            {
                return viewOrder;
            }
            set
            {
                viewOrder = value;
                OnPropertyChanged("ViewOrder");
            }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="mainOpen">Main window</param>
        public EmployeeViewModel(Employee employeeOpen)
        {
            view = employeeOpen;
            using (OrderDBEntities1 context = new OrderDBEntities1())
            {
                OrderList = context.tblOrders.ToList();
            }
        }
        #endregion

        #region Commands

        private ICommand deleteOrder;
        /// <summary>
        /// delete order command
        /// </summary>
        public ICommand DeleteOrder
        {
            get
            {
                if (deleteOrder == null)
                {
                    deleteOrder = new RelayCommand(param => DeleteOrderExecute(), param => CaDeleteOrderExecute());
                }
                return deleteOrder;
            }
        }

        /// <summary>
        /// delete order execute
        /// </summary>
        private void DeleteOrderExecute()
        {
            try
            {
                if (Order != null)
                {
                    int orderId = Order.OrderID;
                    if (Order.OrderStatus == "pending")
                    {
                        service.DenyOrder(orderId);
                        MessageBox.Show("Order denied");
                    }
                    else
                    {
                        service.DeleteOrder(orderId);
                        MessageBox.Show("Order deleted");
                    }
                    using (OrderDBEntities1 context = new OrderDBEntities1())
                    {
                        OrderList = context.tblOrders.ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Can delete order execute
        /// </summary>
        /// <returns>Can or cannot</returns>
        private bool CaDeleteOrderExecute()
        {
            if (Order == null)
                return false;
            else
                return true;
        }

        private ICommand approveOrder;
        /// <summary>
        /// approve order command
        /// </summary>
        public ICommand ApproveOrder
        {
            get
            {
                if (approveOrder == null)
                {
                    approveOrder = new RelayCommand(param => ApproveOrderExecute(), param => CanApproveOrderExecute());
                }
                return approveOrder;
            }
        }

        /// <summary>
        /// approve execute
        /// </summary>
        private void ApproveOrderExecute()
        {
            try
            {
                if (Order != null)
                {
                    int orderId = Order.OrderID;
                    if (Order.OrderStatus == "pending")
                    {
                        service.ApproveOrder(orderId);
                        MessageBox.Show("Order approved");
                    }
                    else if(Order.OrderStatus == "approved")
                    {
                        MessageBox.Show("Order already approved");
                    }
                    else
                    {
                        MessageBox.Show("Can't approve denied order");
                    }
                    using (OrderDBEntities1 context = new OrderDBEntities1())
                    {
                        OrderList = context.tblOrders.ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Can approve order execute
        /// </summary>
        /// <returns>Can or cannot</returns>
        private bool CanApproveOrderExecute()
        {
            if (Order == null)
                return false;
            else
                return true;
        }


        private ICommand logOut;
        /// <summary>
        /// logout command
        /// </summary>
        public ICommand LogOut
        {
            get
            {
                if (logOut == null)
                {
                    logOut = new RelayCommand(param => LogOutExecute(), param => CanLogOutExecute());
                }
                return logOut;
            }
        }

        /// <summary>
        /// logout execute
        /// </summary>
        private void LogOutExecute()
        {
            Login log = new Login();
            log.Show();
            view.Close();
        }

        /// <summary>
        /// Can logout execute
        /// </summary>
        /// <returns>Can or cannot</returns>
        private bool CanLogOutExecute()
        {
            return true;
        }
        #endregion
    }
}
