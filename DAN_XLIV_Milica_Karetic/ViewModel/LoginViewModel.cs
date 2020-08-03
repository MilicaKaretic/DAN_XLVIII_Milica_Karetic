using DAN_XLIV_Milica_Karetic.Commands;
using DAN_XLIV_Milica_Karetic.Model;
using DAN_XLIV_Milica_Karetic.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DAN_XLIV_Milica_Karetic.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        Login view;
        Service service = new Service();

        #region Constructors

        public LoginViewModel(Login view)
        {
            this.view = view;
            user = new tblUser();
            UserList = service.GetAllUsers().ToList();
        }
        #endregion

        #region Property
        private tblUser user;

        public tblUser User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private List<tblUser> userList;

        public List<tblUser> UserList
        {
            get { return userList; }
            set
            {
                userList = value;
                OnPropertyChanged("UserList");
            }
        }

        private string labelInfo;

        public string LabelInfo
        {
            get { return labelInfo; }
            set
            {
                labelInfo = value;
                OnPropertyChanged("LabelInfo");
            }
        }



        #endregion

        #region Commands
        private ICommand login;

        public ICommand Login
        {
            get
            {
                if(login == null)
                {
                    login = new RelayCommand(LoginExecute);
                }
                return login;
            }
            set { login = value; }
        }

        /// <summary>
        /// Checks if its possible to login depending on the given username and password and saves the logged in user to a list
        /// </summary>
        /// <param name="obj"></param>
        private void LoginExecute(object obj)
        {
            string password = (obj as PasswordBox).Password;
            bool found = false;
            
            if (UserList.Any())
            {
                for (int i = 0; i < UserList.Count; i++)
                {
                    if (User.JMBG == UserList[i].JMBG && password == "Gost")
                    {
                        Service.loggedUser.Add(UserList[i]);
                        Service.currentUser = UserList[i];

                        if(service.CheckOrderStatus(UserList[i].UserID))
                        {
                            MessageBox.Show("Your order is pending.");                           
                            Login log = new Login();
                            log.Show();
                            view.Close();
                        }
                        else
                        {
                            MainWindow mw = new MainWindow();
                            LabelInfo = "Logged in";
                            found = true;
                            view.Close();
                            mw.Show();
                            break;
                        }
                        
                    }
                }

                if (found == false)
                {
                    LabelInfo = "Wrong Username or Password";
                }
            }
            else
            {
                LabelInfo = "Database is empty";
            }

            if (User.JMBG == "Zaposleni" && password == "Zaposleni")
            {
                Employee employee = new Employee();
                view.Close();
                employee.Show();
            }
        }

        #endregion
    }
}
