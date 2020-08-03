using DAN_XLIV_Milica_Karetic.Commands;
using DAN_XLIV_Milica_Karetic.Model;
using DAN_XLIV_Milica_Karetic.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        #region JMBG checker


        /// <summary>
        /// Method for validating 13 digits for JMBG
        /// </summary>
        /// <param name="jmbg">Member JMBG</param>
        /// <returns>Valid or invalid</returns>
        public bool ValidJMBG(string jmbg)
        {
            int a = 0;
            for (int i = 0; i < jmbg.Length; i++)
            {
                if (jmbg.Length == 13)
                {
                    if (jmbg[i] >= '0' && jmbg[i] <= '9')
                        a++;
                }
            }
            //calls method to check is entered jmbg is valid
            if (a == 13)
                return CkeckJMBG(jmbg);
            else
            {
               // Console.WriteLine("JMBG must have 13 numbers!!!");
                return false;
            }
        }

        /// <summary>
        /// Method for checking validation for entered jmbg based on formula for JMBG
        /// </summary>
        /// <param name="jmbg">JMBG</param>
        /// <returns>Valid or invalid</returns>
        public bool CkeckJMBG(string jmbg)
        {

            int[] daysOfMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            // input in char array
            char[] array = jmbg.ToCharArray(0, 13);

            // first checks entered year of birth
            // separates digits to get year of birth
            char[] yearOfBirth = jmbg.ToCharArray(4, 3);
            int tempYear = 100 * (Convert.ToInt32(yearOfBirth[0] - '0')) +
                             10 * (Convert.ToInt32(yearOfBirth[1] - '0')) +
                                   Convert.ToInt32(yearOfBirth[2] - '0');

            // if someone is born in XXI century
            if (yearOfBirth[0] == '0')
                tempYear += 2000;
            // who is born in XX century
            else
                tempYear += 1000;

            // year of birth can't be less than 1900
            if (tempYear < 1900)
            {
                //Console.WriteLine("Entered year od birth less then 1900.");
                return false;
            }
            else
            {
                // year of birth can't be bigger than current year 2020
                if (tempYear > DateTime.Now.Year)
                {
                    //Console.WriteLine("Entered year of birth is bigger than current year.");
                    return false;
                }
            }

            // ckecks month of birth
            // separates digits to get month of birth
            char[] monthOfBirth = jmbg.ToCharArray(2, 2);
            int tempMonth = 10 * (Convert.ToInt32(monthOfBirth[0] - '0')) +
                                  Convert.ToInt32(monthOfBirth[1] - '0');
            if (tempMonth > 12 || tempMonth < 1) // month has to be <= 12 and > 0 
            {
                //Console.WriteLine("Wrong entered month of birth (3. and 4. number) !!!");
                return false;
            }

            // ckecks if year if birth is delinquent
            if (((tempYear % 4) == 0) && (((tempYear % 100) != 0) || ((tempYear % 400) == 0)))
            {
                // if is delinquent february has 29 days
                daysOfMonth[1] = 29;
            }

            // ckecks entered day of birth based on month 
            char[] dayOfBirth = jmbg.ToCharArray(0, 2);
            int tempDay = 10 * (Convert.ToInt32(dayOfBirth[0] - '0')) +
                               Convert.ToInt32(dayOfBirth[1] - '0');

            if (tempDay > daysOfMonth[tempMonth - 1] || tempDay < 1)
            {
                //Console.WriteLine("Wrong entered day of birth (1. and 2. number) !!!");
                return false;
            }

            int sum = 0;

            // calculate control sum
            for (int i = 0; i < 6; i++)
                sum += (7 - i) * (Convert.ToInt32(array[i] - '0') + Convert.ToInt32(array[6 + i] - '0'));
            // rest of division sum with 11
            int rest = sum % 11;
            // difference
            int difference = 11 - rest;

            // if rest = 1, JMBG is INVALID
            if (rest == 1)
            {
                //Console.WriteLine("JMBG is INVALID. The rest of division = 1.");
                return false;
            }
            //If rest = 0 and control digit = 0 JMBG is VALID
            else if (rest == 0)
            {
                if (Convert.ToInt32(array[12] - '0').Equals(0))
                {
                    //Console.WriteLine("JMBG is VALID! (rest = 0, control digit = 0)");
                    return true;
                }
                else
                {
                    //Console.WriteLine("JMBG is INVALID! (rest = 0, control digit != 0)");
                    return false;
                }
            }
            else if (difference == (array[12] - '0'))
            {
                //Console.WriteLine("JMBG is VALID! (difference = control digit)");
                return true;
            }
            else
            {
               // Console.WriteLine("JMBG is INVALID! (difference != control digit)");
                return false;
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
            bool correct = false;
            
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
                            if(service.GetOrderStatus(UserList[i].UserID) == "approved")
                            {
                                MessageBox.Show("Your order has been approved");
                                Thread.Sleep(2000);
                            }
                            else if(service.GetOrderStatus(UserList[i].UserID) == "denied")
                            {
                                MessageBox.Show("Your order has been denied");
                                Thread.Sleep(2000);
                            }
                            MainWindow mw = new MainWindow();
                            LabelInfo = "Logged in";
                            found = true;
                            view.Close();
                            mw.Show();
                            break;
                        }
                        correct = true;
                        found = true;
                    }
                }

                if (found == false)
                {
                    if (ValidJMBG(User.JMBG) && password == "Gost")
                    {
                        tblUser user = new tblUser();
                        user.JMBG = User.JMBG;
                        service.AddUser(user);

                        Service.currentUser = service.GetUser(User.JMBG);

                        MainWindow mw = new MainWindow();
                        LabelInfo = "Logged in";
                        found = true;
                        correct = true;
                        view.Close();
                        mw.Show();
                    }
                }
                if (correct == false)
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
