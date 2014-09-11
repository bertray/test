select 
	Id as "Id", 
	Data as "Data"
from tb_r_data
where (Bus = @Bus) and (Id = @Id)