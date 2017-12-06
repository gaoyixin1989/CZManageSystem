using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Uum_Userinfo_LogMap : EntityTypeConfiguration<Uum_Userinfo_Log>
    {
        public Uum_Userinfo_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.employee);
            /// <summary>
            /// �û��ʺ�/��ά��Ա�˺ţ���ְ������ʱ�˺�Ϊ�գ�
            /// <summary>
            this.Property(t => t.userID)

          .HasMaxLength(50);
            /// <summary>
            /// �û�ȫ��
            /// <summary>
            this.Property(t => t.fullName)

          .HasMaxLength(100);
            /// <summary>
            /// ���Ʋ���GUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
            /// <summary>
            this.Property(t => t.OUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������OUGUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
            /// <summary>
            this.Property(t => t.workOUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������(ʡ��˾) 
            /// <summary>
            this.Property(t => t.region)

          .HasMaxLength(100);
            /// <summary>
            /// ������� (OUGUID����)
            /// <summary>
            this.Property(t => t.regionID)

          .HasMaxLength(50);
            /// <summary>
            /// ������˾
            /// <summary>
            this.Property(t => t.company)

          .HasMaxLength(100);
            /// <summary>
            /// ��˾��� (OUGUID����)
            /// <summary>
            this.Property(t => t.companyID)

          .HasMaxLength(50);
            /// <summary>
            /// �����ֹ�˾ 
            /// <summary>
            this.Property(t => t.branch)

          .HasMaxLength(100);
            /// <summary>
            /// �ֹ�˾��� (OUGUID����)
            /// <summary>
            this.Property(t => t.branchID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.department)

          .HasMaxLength(100);
            /// <summary>
            /// ������� (OUGUID����)
            /// <summary>
            this.Property(t => t.departmentID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.userGroup)

          .HasMaxLength(100);
            /// <summary>
            /// ������� (OUGUID����)
            /// <summary>
            this.Property(t => t.userGroupID)

          .HasMaxLength(50);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.hall)

          .HasMaxLength(100);
            /// <summary>
            /// ���������
            /// <summary>
            this.Property(t => t.hallid)

          .HasMaxLength(50);
            /// <summary>
            /// ���۵㣨�ࡢ�飩
            /// <summary>
            this.Property(t => t.salepoint)

          .HasMaxLength(100);
            /// <summary>
            /// ���۵����(portal����)
            /// <summary>
            this.Property(t => t.salepointid)

          .HasMaxLength(50);
            /// <summary>
            /// ����ֶ�֮ǰ���գ���ΪӪҵ����Ա������ݴ洢ʹ����Ҫ�洢BOSS���ţ�BOSS���������͵ȣ�ʹ��@@@�ָ���BOSS����@@@��½����@@@Ա������@@@�˺�״̬@@@ BOSS���Ų���Ա���� @@@����ʱ��(ʱ���ʽ:YYYY-MM-DD)@@@��������@@@��������
            ///(�����˺�״̬ȡֵΪ1��2��1Ϊ��Ч��2ΪʧЧ)
            ///ͣ��ʱ����ʹ��(userQuitDate)
            /// <summary>
            this.Property(t => t.teams)

          .HasMaxLength(255);
            /// <summary>
            /// ��Ա������ һ��������Ӧ��ϵͳֻ��Ҫ��Ч���û����ݣ���jobtypeΪ000001���ڸ���Ա�⣩000004����ữԱ���⣩��000009��ȫʡ������Ա�⣩��000010��ȫʡ�����Ա�⣩��000088����ά��Ա�⣩��000099����ʱ�����Ա�⣩000003 �������ְ��Ա�⣩000002 (������Ա) 000006(��ְ��Ա�⣩000007��������Ա�⣩
            ///000066(�ͷ���Ա�⣩000077��Ӫҵ����Ա�⣩000055(ϵͳ�˺ţ�000044 (�����˺ţ�
            /// <summary>
            this.Property(t => t.jobType)

          .HasMaxLength(20);
            /// <summary>
            /// ��Ա������-PORTAL(�ο���¼)
            /// <summary>
            this.Property(t => t.userType)

          .HasMaxLength(20);
            /// <summary>
            /// �����绰
            /// <summary>
            this.Property(t => t.workPhone)

          .HasMaxLength(50);
            /// <summary>
            /// �ƶ��绰
            /// <summary>
            this.Property(t => t.telePhone)

          .HasMaxLength(30);
            /// <summary>
            /// �칫�ʼ�
            /// <summary>
            this.Property(t => t.email)

          .HasMaxLength(128);
            /// <summary>
            /// ��ϵ��ַ
            /// <summary>
            this.Property(t => t.address)

          .HasMaxLength(255);
            /// <summary>
            /// �Ա�(�ο���¼)
            /// <summary>
            this.Property(t => t.sex)

          .HasMaxLength(1);
            /// <summary>
            /// ְλ
            /// <summary>
            this.Property(t => t.title)

          .HasMaxLength(80);
            /// <summary>
            /// ���Ŷ̺�
            /// <summary>
            this.Property(t => t.shortMobile)

          .HasMaxLength(12);
            /// <summary>
            /// �û�ְλ�ȼ�(�ο���¼)
            /// <summary>
            this.Property(t => t.userPosiLevel)

          .HasMaxLength(2);
            /// <summary>
            /// ����ְ��
            /// <summary>
            this.Property(t => t.currentLevel)

          .HasMaxLength(2);
            /// <summary>
            /// �û�ְλ (�ο���¼)
            /// <summary>
            this.Property(t => t.employeeClass)

          .HasMaxLength(50);
            /// <summary>
            /// Ա��ְ��(�ο���¼)
            /// <summary>
            this.Property(t => t.userGrade)

          .HasMaxLength(100);
            /// <summary>
            /// cmccaccount
            /// <summary>
            this.Property(t => t.CMCCAccount)

          .HasMaxLength(100);
            /// <summary>
            /// ����ID����UI����ʾʱ��������
            /// <summary>
            this.Property(t => t.orderID)

          .HasMaxLength(255);
            /// <summary>
            /// ����(�ο���¼)
            /// <summary>
            this.Property(t => t.userNation)

          .HasMaxLength(5);
            /// <summary>
            /// ������ò(�ο���¼)
            /// <summary>
            this.Property(t => t.userReligion)

          .HasMaxLength(5);
            /// <summary>
            /// �û�DN
            /// <summary>
            this.Property(t => t.userDN)

          .HasMaxLength(255);
            /// <summary>
            /// ְ������
            /// <summary>
            this.Property(t => t.dutyDesc)

          .HasMaxLength(200);
            /// <summary>
            /// �û�������LDAP�б䶯��ʱ��
            /// <summary>
            this.Property(t => t.changeTime)

          .HasMaxLength(255);
            /// <summary>
            /// 3G�绰�������ڴ�άϵͳ������ID��
            /// <summary>
            this.Property(t => t.PHONE3G)

          .HasMaxLength(20);
            /// <summary>
            /// ��֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            /// <summary>
            this.Property(t => t.OrgID)

          .HasMaxLength(50);
            /// <summary>
            /// ��־����(ADD��UPT��DEL)
            /// <summary>
            this.Property(t => t.operationType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("UUM_USERINFO_LOG");
            // �û����Ψһ��ʶ��2λ��˾����+8λԱ�����,��ά��ԱΪһ��10λ�����ֱ���
            this.Property(t => t.employee).HasColumnName("employee");
            // �û��ʺ�/��ά��Ա�˺ţ���ְ������ʱ�˺�Ϊ�գ�
            this.Property(t => t.userID).HasColumnName("userID");
            // �û�ȫ��
            this.Property(t => t.fullName).HasColumnName("fullName");
            // ���Ʋ���PortalID����
            this.Property(t => t.OUID).HasColumnName("OUID");
            // ���Ʋ���GUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
            this.Property(t => t.OUGUID).HasColumnName("OUGUID");
            // ��������OUGUID�����ṩ���ݣ���Է�PORTALӦ�ã�/��ά��ԱΪ�����Ŷ�
            this.Property(t => t.workOUGUID).HasColumnName("workOUGUID");
            // ��������(ʡ��˾) 
            this.Property(t => t.region).HasColumnName("region");
            // ������� (OUGUID����)
            this.Property(t => t.regionID).HasColumnName("regionID");
            // ������˾
            this.Property(t => t.company).HasColumnName("company");
            // ��˾��� (OUGUID����)
            this.Property(t => t.companyID).HasColumnName("companyID");
            // �����ֹ�˾ 
            this.Property(t => t.branch).HasColumnName("branch");
            // �ֹ�˾��� (OUGUID����)
            this.Property(t => t.branchID).HasColumnName("branchID");
            // ��������
            this.Property(t => t.department).HasColumnName("department");
            // ������� (OUGUID����)
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            // ��������
            this.Property(t => t.userGroup).HasColumnName("userGroup");
            // ������� (OUGUID����)
            this.Property(t => t.userGroupID).HasColumnName("userGroupID");
            // ������
            this.Property(t => t.hall).HasColumnName("hall");
            // ���������
            this.Property(t => t.hallid).HasColumnName("hallid");
            // ���۵㣨�ࡢ�飩
            this.Property(t => t.salepoint).HasColumnName("salepoint");
            // ���۵����(portal����)
            this.Property(t => t.salepointid).HasColumnName("salepointid");
            // ����ֶ�֮ǰ���գ���ΪӪҵ����Ա������ݴ洢ʹ����Ҫ�洢BOSS���ţ�BOSS���������͵ȣ�ʹ��@@@�ָ���BOSS����@@@��½����@@@Ա������@@@�˺�״̬@@@ BOSS���Ų���Ա���� @@@����ʱ��(ʱ���ʽ:YYYY-MM-DD)@@@��������@@@��������
            //(�����˺�״̬ȡֵΪ1��2��1Ϊ��Ч��2ΪʧЧ)
            //ͣ��ʱ����ʹ��(userQuitDate)

            this.Property(t => t.teams).HasColumnName("teams");
            // ��Ա������ һ��������Ӧ��ϵͳֻ��Ҫ��Ч���û����ݣ���jobtypeΪ000001���ڸ���Ա�⣩000004����ữԱ���⣩��000009��ȫʡ������Ա�⣩��000010��ȫʡ�����Ա�⣩��000088����ά��Ա�⣩��000099����ʱ�����Ա�⣩000003 �������ְ��Ա�⣩000002 (������Ա) 000006(��ְ��Ա�⣩000007��������Ա�⣩
            //000066(�ͷ���Ա�⣩000077��Ӫҵ����Ա�⣩000055(ϵͳ�˺ţ�000044(�����˺ţ�

            this.Property(t => t.jobType).HasColumnName("jobType");
            // ��Ա������-PORTAL(�ο���¼)
            this.Property(t => t.userType).HasColumnName("userType");
            // �����绰
            this.Property(t => t.workPhone).HasColumnName("workPhone");
            // �ƶ��绰
            this.Property(t => t.telePhone).HasColumnName("telePhone");
            // �칫�ʼ�
            this.Property(t => t.email).HasColumnName("email");
            // ��ϵ��ַ
            this.Property(t => t.address).HasColumnName("address");
            // ����
            this.Property(t => t.userBirthday).HasColumnName("userBirthday");
            // �Ա�(�ο���¼)
            this.Property(t => t.sex).HasColumnName("sex");
            // ְλ
            this.Property(t => t.title).HasColumnName("title");
            // ���Ŷ̺�
            this.Property(t => t.shortMobile).HasColumnName("shortMobile");
            // �û�ְλ�ȼ�(�ο���¼)
            this.Property(t => t.userPosiLevel).HasColumnName("userPosiLevel");
            // ����ְ��
            this.Property(t => t.currentLevel).HasColumnName("currentLevel");
            // �û�ְλ (�ο���¼)
            this.Property(t => t.employeeClass).HasColumnName("employeeClass");
            // Ա��ְ��(�ο���¼)
            this.Property(t => t.userGrade).HasColumnName("userGrade");
            // cmccaccount
            this.Property(t => t.CMCCAccount).HasColumnName("CMCCAccount");
            // ����ID����UI����ʾʱ��������
            this.Property(t => t.orderID).HasColumnName("orderID");
            // ���빫˾����
            this.Property(t => t.userJoinInDate).HasColumnName("userJoinInDate");
            // ����(�ο���¼)
            this.Property(t => t.userNation).HasColumnName("userNation");
            // ������ò(�ο���¼)
            this.Property(t => t.userReligion).HasColumnName("userReligion");
            // �뿪��˾���ڣ������û��˺�ʹ�õĸ����ֶΣ�
            this.Property(t => t.userQuitDate).HasColumnName("userQuitDate");
            // �û�DN
            this.Property(t => t.userDN).HasColumnName("userDN");
            // ְ������
            this.Property(t => t.dutyDesc).HasColumnName("dutyDesc");
            // �û�������LDAP�б䶯��ʱ��
            this.Property(t => t.changeTime).HasColumnName("changeTime");
            // ��¼��������
            this.Property(t => t.createTime).HasColumnName("createTime");
            // ����޸�����
            this.Property(t => t.lastModifyTime).HasColumnName("lastModifyTime");
            // 3G�绰�������ڴ�άϵͳ������ID��
            this.Property(t => t.PHONE3G).HasColumnName("PHONE3G");
            // ��֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            this.Property(t => t.OrgID).HasColumnName("OrgID");
            // ��־ID
            this.Property(t => t.logID).HasColumnName("logID");
            // ��־����(ADD��UPT��DEL)
            this.Property(t => t.operationType).HasColumnName("operationType");
        }
    }
}
