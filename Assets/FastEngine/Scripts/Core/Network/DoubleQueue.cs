/*
 * @Author: fasthro
 * @Date: 2019-10-22 16:20:49
 * @Description: 双队列(多线程编程中，能保证生产者线程的写入和消费者的读出尽量做到最低的影响，避免了共享队列的锁开销)
 */
using System.Collections;

namespace FastEngine.Core
{
    public class DoubleQueue<T> where T : class
    {
        // 消费者
        private Queue m_consume;
        // 生产者
        private Queue m_produce;

        /// <summary>
        /// 双队列
        /// </summary>
        /// <param name="capcity"></param>
        public DoubleQueue(int capcity = 16)
        {
            m_consume = new Queue(capcity);
            m_produce = new Queue(capcity);
        }

        public void Enqueue(T arg)
        {
            lock (m_produce)
            {
                m_produce.Enqueue(arg);
            }
        }

        public T Dequeue()
        {
            return m_consume.Dequeue() as T;
        }

        public void Swap()
        {
            lock (m_produce)
            {
                Queue temp = m_consume;
                m_consume = m_produce;
                m_produce = temp;
            }
        }

        public void Clear()
        {
            lock(m_produce)
            {
                m_produce.Clear();
                m_consume.Clear();
            }
        }

        public bool IsEmpty()
        {
            return m_consume.Count == 0;
        }
    }
}