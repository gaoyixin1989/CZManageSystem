using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class Uum_Userinfo_Log
    {
        /// <summary>
        /// �û����Ψһ��ʶ��2λ��˾����+8λԱ�����,��ά��ԱΪһ��10λ�����ֱ���
        /// <summary>
        public string employee { get; set; }
        /// <summary>
        /// �û��ʺ�/��ά��Ա�˺ţ���ְ������ʱ�˺�Ϊ�գ�
        /// <summary>
        public string userID { get; set; }
        /// <summary>
        /// �û�ȫ��
        /// <summary>
        public string fullName { get; set; }
        /// <summary>
        /// ���Ʋ���PortalID����
        /// <summary>
        public Nullable<decimal> OUID { get; set; }
        /// <summary>
        /// ���Ʋ���GUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
        /// <summary>
        public string OUGUID { get; set; }
        /// <summary>
        /// ��������OUGUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
        /// <summary>
        public string workOUGUID { get; set; }
        /// <summary>
        /// ��������(ʡ��˾) 
        /// <summary>
        public string region { get; set; }
        /// <summary>
        /// ������� (OUGUID����)
        /// <summary>
        public string regionID { get; set; }
        /// <summary>
        /// ������˾
        /// <summary>
        public string company { get; set; }
        /// <summary>
        /// ��˾��� (OUGUID����)
        /// <summary>
        public string companyID { get; set; }
        /// <summary>
        /// �����ֹ�˾ 
        /// <summary>
        public string branch { get; set; }
        /// <summary>
        /// �ֹ�˾��� (OUGUID����)
        /// <summary>
        public string branchID { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string department { get; set; }
        /// <summary>
        /// ������� (OUGUID����)
        /// <summary>
        public string departmentID { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string userGroup { get; set; }
        /// <summary>
        /// ������� (OUGUID����)
        /// <summary>
        public string userGroupID { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string hall { get; set; }
        /// <summary>
        /// ���������
        /// <summary>
        public string hallid { get; set; }
        /// <summary>
        /// ���۵㣨�ࡢ�飩
        /// <summary>
        public string salepoint { get; set; }
        /// <summary>
        /// ���۵����(portal����)
        /// <summary>
        public string salepointid { get; set; }
        /// <summary>
        /// ����ֶ�֮ǰ���գ���ΪӪҵ����Ա������ݴ洢ʹ����Ҫ�洢BOSS���ţ�BOSS���������͵ȣ�ʹ��@@@�ָ���BOSS����@@@��½����@@@Ա������@@@�˺�״̬@@@ BOSS���Ų���Ա���� @@@����ʱ��(ʱ���ʽ:YYYY-MM-DD)@@@��������@@@��������
        ///(�����˺�״̬ȡֵΪ1��2��1Ϊ��Ч��2ΪʧЧ)
        ///ͣ��ʱ����ʹ��(userQuitDate)
        /// <summary>
        public string teams { get; set; }
        /// <summary>
        /// ��Ա������ һ��������Ӧ��ϵͳֻ��Ҫ��Ч���û����ݣ���jobtypeΪ000001���ڸ���Ա�⣩000004����ữԱ���⣩��000009��ȫʡ������Ա�⣩��000010��ȫʡ�����Ա�⣩��000088����ά��Ա�⣩��000099����ʱ�����Ա�⣩000003 �������ְ��Ա�⣩000002 (������Ա) 000006(��ְ��Ա�⣩000007��������Ա�⣩
        ///000066(�ͷ���Ա�⣩000077��Ӫҵ����Ա�⣩000055(ϵͳ�˺ţ�000044 (�����˺ţ�
        /// <summary>
        public string jobType { get; set; }
        /// <summary>
        /// ��Ա������-PORTAL(�ο���¼)
        /// <summary>
        public string userType { get; set; }
        /// <summary>
        /// �����绰
        /// <summary>
        public string workPhone { get; set; }
        /// <summary>
        /// �ƶ��绰
        /// <summary>
        public string telePhone { get; set; }
        /// <summary>
        /// �칫�ʼ�
        /// <summary>
        public string email { get; set; }
        /// <summary>
        /// ��ϵ��ַ
        /// <summary>
        public string address { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public Nullable<DateTime> userBirthday { get; set; }
        /// <summary>
        /// �Ա�(�ο���¼)
        /// <summary>
        public string sex { get; set; }
        /// <summary>
        /// ְλ
        /// <summary>
        public string title { get; set; }
        /// <summary>
        /// ���Ŷ̺�
        /// <summary>
        public string shortMobile { get; set; }
        /// <summary>
        /// �û�ְλ�ȼ�(�ο���¼)
        /// <summary>
        public string userPosiLevel { get; set; }
        /// <summary>
        /// ����ְ��
        /// <summary>
        public string currentLevel { get; set; }
        /// <summary>
        /// �û�ְλ (�ο���¼)
        /// <summary>
        public string employeeClass { get; set; }
        /// <summary>
        /// Ա��ְ��(�ο���¼)
        /// <summary>
        public string userGrade { get; set; }
        /// <summary>
        /// cmccaccount
        /// <summary>
        public string CMCCAccount { get; set; }
        /// <summary>
        /// ����ID����UI����ʾʱ��������
        /// <summary>
        public string orderID { get; set; }
        /// <summary>
        /// ���빫˾����
        /// <summary>
        public Nullable<DateTime> userJoinInDate { get; set; }
        /// <summary>
        /// ����(�ο���¼)
        /// <summary>
        public string userNation { get; set; }
        /// <summary>
        /// ������ò(�ο���¼)
        /// <summary>
        public string userReligion { get; set; }
        /// <summary>
        /// �뿪��˾���ڣ������û��˺�ʹ�õĸ����ֶΣ�
        /// <summary>
        public Nullable<DateTime> userQuitDate { get; set; }
        /// <summary>
        /// �û�DN
        /// <summary>
        public string userDN { get; set; }
        /// <summary>
        /// ְ������
        /// <summary>
        public string dutyDesc { get; set; }
        /// <summary>
        /// �û�������LDAP�б䶯��ʱ��
        /// <summary>
        public string changeTime { get; set; }
        /// <summary>
        /// ��¼��������
        /// <summary>
        public Nullable<DateTime> createTime { get; set; }
        /// <summary>
        /// ����޸�����
        /// <summary>
        public Nullable<DateTime> lastModifyTime { get; set; }
        /// <summary>
        /// 3G�绰�������ڴ�άϵͳ������ID��
        /// <summary>
        public string PHONE3G { get; set; }
        /// <summary>
        /// ��֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
        /// <summary>
        public string OrgID { get; set; }
        /// <summary>
        /// ��־ID
        /// <summary>
        public Nullable<decimal> logID { get; set; }
        /// <summary>
        /// ��־����(ADD��UPT��DEL)
        /// <summary>
        public string operationType { get; set; }

    }
}
