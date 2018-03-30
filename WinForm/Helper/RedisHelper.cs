using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceKit.Redis;
using StackExchange.Redis;

namespace Helper
{
    /// <summary>
    /// Redis 操作类
    /// </summary>
    public class RedisHelper
    {


        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString;
        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _lock = new object();
        private readonly static object obj = new object();

        public static IRedisClientsManager clientManager = new PooledRedisClientManager(ConnectionString
         );
        public static IRedisClient redisClent = clientManager.GetClient();

        private static RedisHelper _instance;

        public static RedisHelper GetInstance()
        {
            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new RedisHelper();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// 连接对象
        /// </summary>
        private volatile IConnectionMultiplexer _connection;
        /// <summary>
        /// 数据库
        /// </summary>
        private IDatabase _db;
        public RedisHelper()
        {
            _connection = ConnectionMultiplexer.Connect(ConnectionString);
            _db = GetDatabase();
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected IConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
                _connection = ConnectionMultiplexer.Connect(ConnectionString);
            }

            return _connection;
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }
        /// <summary>
        /// 设置缓存 时间秒
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">时间(秒)</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            var entryBytes = Serialize(data.ToString());
            var expiresIn = TimeSpan.FromSeconds(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }
        /// <summary>
        /// 设置缓存 时间秒
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">时间(秒)</param>
        public virtual void SetIntBySec(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromSeconds(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }
        /// <summary>
        /// 设置缓存 时间小时
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">时间(秒)</param>
        public virtual void SetByHourValue(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }
            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromHours(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {

            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
            {
                return default(T);
            }

            var result = Deserialize<T>(rValue);

            return result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }
            var json = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 判断是否已经设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _db.KeyExists(key);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        private byte[] Serialize(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Enque(string key, string value)
        {
            redisClent.EnqueueItemOnList(key, value);
        }

        /// <summary>
        /// 出队列
        /// </summary>
        /// <param name="key"></param>
        public string Deque(string key)
        {
        
           return redisClent.DequeueItemFromList(key);
        }

        /// <summary>
        /// 返回队列数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int QueueCount(string key)
        {
            return redisClent.GetAllItemsFromList(key).Count();
        }

    }

}
