using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class Uum_Organizationinfo
    {
        /// <summary>
        /// ��֯ID(���APP)
        /// <summary>
        public string OUGUID { get; set; }
        /// <summary>
        /// ����֯ID(���APP)
        /// <summary>
        public string parentOUGUID { get; set; }
        /// <summary>
        /// ��֯ID (portal),����portal���ֲ��Ƽ�ʹ��
        /// <summary>
        public Nullable<decimal> OUID { get; set; }
        /// <summary>
        /// ��֯����(UN: ��˾  UM: ����,TD:�Ŷ�)
        /// <summary>
        public string orgTypeID { get; set; }
        /// <summary>
        /// ��֯״̬��1 ����  0ͣ�ã�
        /// <summary>
        public string orgState { get; set; }
        /// <summary>
        /// ��֯����(portal)/�Ŷ�����
        /// <summary>
        public string OUName { get; set; }
        /// <summary>
        /// �ϼ���֯ID(portal), ����portal���ֲ��Ƽ�ʹ��
        /// <summary>
        public Nullable<decimal> parentOUID { get; set; }
        /// <summary>
        /// ��֯ȫ��(portal)
        /// <summary>
        public string OUFullName { get; set; }
        /// <summary>
        /// ��֯���(portal)
        /// <summary>
        public Nullable<decimal> OULevel { get; set; }
        /// <summary>
        /// ��֯����(portal)
        /// <summary>
        public string OUOrder { get; set; }
        /// <summary>
        /// ��֯�е�DN(portal)
        /// <summary>
        public string OrgDN { get; set; }
        /// <summary>
        /// ��������(ʡ��˾)
        /// <summary>
        public string region { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string regionID { get; set; }
        /// <summary>
        /// ������˾
        /// <summary>
        public string company { get; set; }
        /// <summary>
        /// ��˾���
        /// <summary>
        public string companyID { get; set; }
        /// <summary>
        /// �����ֹ�˾
        /// <summary>
        public string branch { get; set; }
        /// <summary>
        /// �ֹ�˾���
        /// <summary>
        public string branchID { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string department { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string departmentID { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string userGroup { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string userGroupID { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string hall { get; set; }
        /// <summary>
        /// ������ID
        /// <summary>
        public string hallid { get; set; }
        /// <summary>
        /// ���۵㣨�ࡢ�飩
        /// <summary>
        public string salepoint { get; set; }
        /// <summary>
        /// ���۵�ID
        /// <summary>
        public string salepointid { get; set; }
        /// <summary>
        /// �����飨�������Ӣ�Ķ��ŷָ���������֯�Ǵ�ά�Ŷӣ����ֵΪ��Ӧ��ID���ƶ��ӿ��ˣ�ʾ����11992233,wuyuming
        ///����֯������������ֵ��Ϊ�������룬ʾ����ZQFN0014
        /// <summary>
        public string teams { get; set; }
        /// <summary>
        /// ��¼��������
        /// <summary>
        public Nullable<DateTime> createTime { get; set; }
        /// <summary>
        /// ����޸�����
        /// <summary>
        public Nullable<DateTime> lastModifyTime { get; set; }

    }
}
