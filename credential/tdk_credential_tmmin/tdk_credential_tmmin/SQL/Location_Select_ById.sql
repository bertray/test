select	 location_id as Id
		,location as Name
from tb_m_location
where (company_id = @CompanyId) and (location_id = @Id)