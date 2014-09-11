select	 class_id as Id
		,class_name as Name
from tb_m_class
where (company_id = @CompanyId) and (class_id = @Id)