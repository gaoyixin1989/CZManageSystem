using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Uum_Organizationinfo_LogMap : EntityTypeConfiguration<Uum_Organizationinfo_Log>
    {
        public Uum_Organizationinfo_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.OUGUID);
            /// <summary>
            /// ����֯ID(���APP)
            /// <summary>
            this.Property(t => t.parentOUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// ��֯����(UN: ��˾  UM: ����,TD:�Ŷ�)
            /// <summary>
            this.Property(t => t.orgTypeID)

          .HasMaxLength(50);
            /// <summary>
            /// ��֯״̬��1 ����  0ͣ�ã�
            /// <summary>
            this.Property(t => t.orgState)

          .HasMaxLength(50);
            /// <summary>
            /// ��֯����(portal)/�Ŷ�����
            /// <summary>
            this.Property(t => t.OUName)

          .HasMaxLength(100);
            /// <summary>
            /// ��֯ȫ��(portal)
            /// <summary>
            this.Property(t => t.OUFullName)

          .HasMaxLength(255);
            /// <summary>
            /// ��֯����(portal)
            /// <summary>
            this.Property(t => t.OUOrder)

          .HasMaxLength(255);
            /// <summary>
            /// ��֯�е�DN(portal)
            /// <summary>
            this.Property(t => t.OrgDN)

          .HasMaxLength(255);
            /// <summary>
            /// ��������(ʡ��˾)
            /// <summary>
            this.Property(t => t.region)

          .HasMaxLength(100);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.regionID)

          .HasMaxLength(50);
            /// <summary>
            /// ������˾
            /// <summary>
            this.Property(t => t.company)

          .HasMaxLength(100);
            /// <summary>
            /// ��˾���
            /// <summary>
            this.Property(t => t.companyID)

          .HasMaxLength(50);
            /// <summary>
            /// �����ֹ�˾
            /// <summary>
            this.Property(t => t.branch)

          .HasMaxLength(100);
            /// <summary>
            /// �ֹ�˾���
            /// <summary>
            this.Property(t => t.branchID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.department)

          .HasMaxLength(100);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.departmentID)

          .HasMaxLength(50);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.userGroup)

          .HasMaxLength(100);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.userGroupID)

          .HasMaxLength(50);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.hall)

          .HasMaxLength(100);
            /// <summary>
            /// ������ID
            /// <summary>
            this.Property(t => t.hallid)

          .HasMaxLength(50);
            /// <summary>
            /// ���۵㣨�ࡢ�飩
            /// <summary>
            this.Property(t => t.salepoint)

          .HasMaxLength(100);
            /// <summary>
            /// ���۵�ID
            /// <summary>
            this.Property(t => t.salepointid)

          .HasMaxLength(50);
            /// <summary>
            /// �����飨�������Ӣ�Ķ��ŷָ���������֯�Ǵ�ά�Ŷӣ����ֵΪ��Ӧ��ID���ƶ��ӿ��ˣ�ʾ����11992233,wuyuming
            ///����֯������������ֵ��Ϊ�������룬ʾ����ZQFN0014
            /// <summary>
            this.Property(t => t.teams)

          .HasMaxLength(255);
            
            /// <summary>
            /// ��־����(ADD��UPT��DEL)
            /// <summary>
            this.Property(t => t.operationType).HasMaxLength(50);
            /// <summary>
            /// ��֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            /// <summary>
            this.Property(t => t.OrgID).HasMaxLength(50);
            /// <summary>
            /// ��֯����(����SMAPƽ̨������ϵͳ��ʹ��)
            /// <summary>
            this.Property(t => t.OrgName).HasMaxLength(100);
            /// <summary>
            /// ����֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            /// <summary>
            this.Property(t => t.ParentOrgID).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("UUM_ORGANIZATIONINFO_LOG");
            // ��֯ID(���APP)
            this.Property(t => t.OUGUID).HasColumnName("OUGUID");
            // ����֯ID(���APP)
            this.Property(t => t.parentOUGUID).HasColumnName("parentOUGUID");
            // ��֯ID (portal),����portal���ֲ��Ƽ�ʹ��
            this.Property(t => t.OUID).HasColumnName("OUID");
            // ��֯����(UN: ��˾  UM: ����,TD:�Ŷ�)
            this.Property(t => t.orgTypeID).HasColumnName("orgTypeID");
            // ��֯״̬��1 ����  0ͣ�ã�
            this.Property(t => t.orgState).HasColumnName("orgState");
            // ��֯����(portal)/�Ŷ�����
            this.Property(t => t.OUName).HasColumnName("OUName");
            // �ϼ���֯ID(portal), ����portal���ֲ��Ƽ�ʹ��
            this.Property(t => t.parentOUID).HasColumnName("parentOUID");
            // ��֯ȫ��(portal)
            this.Property(t => t.OUFullName).HasColumnName("OUFullName");
            // ��֯���(portal)
            this.Property(t => t.OULevel).HasColumnName("OULevel");
            // ��֯����(portal)
            this.Property(t => t.OUOrder).HasColumnName("OUOrder");
            // ��֯�е�DN(portal)
            this.Property(t => t.OrgDN).HasColumnName("OrgDN");
            // ��������(ʡ��˾)
            this.Property(t => t.region).HasColumnName("region");
            // �������
            this.Property(t => t.regionID).HasColumnName("regionID");
            // ������˾
            this.Property(t => t.company).HasColumnName("company");
            // ��˾���
            this.Property(t => t.companyID).HasColumnName("companyID");
            // �����ֹ�˾
            this.Property(t => t.branch).HasColumnName("branch");
            // �ֹ�˾���
            this.Property(t => t.branchID).HasColumnName("branchID");
            // ��������
            this.Property(t => t.department).HasColumnName("department");
            // �������
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            // ��������
            this.Property(t => t.userGroup).HasColumnName("userGroup");
            // �������
            this.Property(t => t.userGroupID).HasColumnName("userGroupID");
            // ������
            this.Property(t => t.hall).HasColumnName("hall");
            // ������ID
            this.Property(t => t.hallid).HasColumnName("hallid");
            // ���۵㣨�ࡢ�飩
            this.Property(t => t.salepoint).HasColumnName("salepoint");
            // ���۵�ID
            this.Property(t => t.salepointid).HasColumnName("salepointid");
            // �����飨�������Ӣ�Ķ��ŷָ���������֯�Ǵ�ά�Ŷӣ����ֵΪ��Ӧ��ID���ƶ��ӿ��ˣ�ʾ����11992233,wuyuming
            //����֯������������ֵ��Ϊ�������룬ʾ����ZQFN0014
            this.Property(t => t.teams).HasColumnName("teams");
            // ��¼��������
            this.Property(t => t.createTime).HasColumnName("createTime");
            // ����޸�����
            this.Property(t => t.lastModifyTime).HasColumnName("lastModifyTime");
            // ��־ID
            this.Property(t => t.logID).HasColumnName("logID");
            // ��־����(ADD��UPT��DEL)
            this.Property(t => t.operationType).HasColumnName("operationType");
            // ��֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            this.Property(t => t.OrgID).HasColumnName("OrgID");
            // ��֯����(����SMAPƽ̨������ϵͳ��ʹ��)
            this.Property(t => t.OrgName).HasColumnName("OrgName");
            // ����֯ID(����SMAPƽ̨������ϵͳ��ʹ��)
            this.Property(t => t.ParentOrgID).HasColumnName("ParentOrgID");
        }
    }
}
