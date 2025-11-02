#include <stdio.h>


int main(){

	int num = 1; 
	char *p = (char*)&num;
	
	if(*p == 1){
		printf("Little endian\n");
	}
	else{
		printf("Big endian\n");
	}
	


	return 0 ;

}
