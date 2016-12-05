using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //做的第一个操作
            //1,声明一个数据库实体上下文类
            hbxlsEntities dbContext = new hbxlsEntities();


            #region 添加
            //2,声明一个User实体
            wx_xx_user user1 = new wx_xx_user();
            user1.cnname = "肖雄";
            user1.open_id = "ddd";
            user1.shop_id = 123;


            //3 ,告诉EF 对上面的实体做一个插入操作

            //dbContext.wx_xx_user.Add(user1);

            //4,告诉上下午把实体的变化保存到数据库
            //dbContext.SaveChanges();

            #endregion

            #region 更新

            wx_xx_user user2 = new wx_xx_user();
            user2.Id = 2;
            user2.open_id = "ccc";
            user2.cnname = "aaa";
            user2.shop_id = 1111;

            //告诉上下文对此实体进行更新
            //dbContext.Entry<wx_xx_user>(user2).State = System.Data.Entity.EntityState.Modified;

            //只修改列
            //dbContext.wx_xx_user.Attach(user2);
            //dbContext.Entry<wx_xx_user>(user2).Property<int>(u => u.shop_id).IsModified = true;
            //保存
            //dbContext.SaveChanges();


            #region 查询
            foreach(var user in dbContext.wx_xx_user)
            {
                Console.WriteLine(user.cnname);
            }
            #endregion


            #region Linq查询

            var temp = from u in dbContext.wx_xx_user
                       where u.Id>4
                       select u;
            foreach (var item in temp)
            {
                Console.WriteLine(item.Id+"-----"+item.cnname);
            }


            #endregion

            #endregion

            Console.ReadKey();
        }
    }
}
