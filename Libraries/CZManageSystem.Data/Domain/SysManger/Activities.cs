using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class Activities
	{        
        public Guid WorkflowId { get; set; }//���̻ID
        public Guid ActivityId { get; set; }//����ID
        public string ActivityName { get; set; }//���̻����
        public int State { get; set; }//���̻��״̬
        public Nullable<int> SortOrder { get; set;}//
		public Nullable<Guid> PrevActivitySetId { get; set; }//��һ�����ID
        public Nullable<Guid> NextActivitySetId { get; set; }//��һ�����ID
        public string JoinCondition { get; set; }//�ϲ�����
        public string SplitCondition { get; set; }//��֧����
        public string CommandRules { get; set; }//����������ִ�й���
        public string ExecutionHandler { get; set; }//�ִ���߼���ʵ������
        public string PostHandler { get; set; }//�ִ�к�ĺ��������ʵ������
        public string AllocatorResource { get; set; }//������Դ�����ڷ�������Ĭ�����ƣ�resource
        public string AllocatorUsers { get; set; }//�ɷ���������û���Ĭ�����ƣ�users
        public string ExtendAllocators { get; set; }//��չ���������ʵ������
        public string ExtendAllocatorArgs { get; set; }//��չ���������ʵ������������ʽ
        public string DefaultAllocator { get; set; }//Ĭ�ϵ��������ʵ����������.
        public string DecisionType { get; set; }//��֧�������ͣ�manual�ֶ�
                                                //        auto�Զ�
                                                //Ĭ��Ϊmanual
                                                //(��Щ�����Ҫ����·��ѡ��ֱ�Ӹ��������ľ���)
        public string DecisionParser { get; set;}//
		public string CountersignedCondition { get; set;}//
		public Nullable<Guid> ParallelActivitySetId { get; set;}//
		public string RejectOption { get; set;}//
		public Nullable<int> CanPrint { get; set;}//
		public Nullable<int> PrintAmount { get; set;}//
		public Nullable<int> CanEdit { get; set;}//
		public Nullable<bool> ReturnToPrev { get; set;}//
		public Nullable<bool> IsMobile { get; set;}//
		public Nullable<bool> IsTimeOutContinue { get; set;}//


        //public virtual ICollection<Tracking_Activities_Completed> Tracking_Activities_Completeds { get; set; }
        public virtual ICollection<ActivitySet> ActivitySets { get; set; }

    }
}
