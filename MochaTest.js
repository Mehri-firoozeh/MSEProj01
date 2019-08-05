console.log('starting test');
var chai = require('chai');
var chaiHttp = require('chai-http');
var async = require('async');
var chaiJsonEqual = require('chai-json-equal');

var assert = chai.assert;
var expect = chai.expect;
var should = chai.should();
chai.use(chaiHttp);



process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

var cookie = 'ai_user=X8pZz|2016-04-10T03:09:45.992Z; ARRAffinity=5d61d7e367d553a3c54df0aaec20b2ad4cadb57598e9e108062da519ca725375; ASP.NET_SessionId=xlofspejffca5uw4o0bsjlj2; .AspNet.ExternalCookie=5ULO1XM5wFoReS0fLZ9psmhsDtJgedNRGSOqBYOfim6OkExzhac0-HiasB-iVkbzkHEWf2uudenqys0G9crY5G3jIiU_o0Q9BEc2aOmjJHp-JxMdY8kslyByOjo5BdtNDliUklIuYcshWCwsCDG14IBIsMOwvIbrFI6fjBhE62J_SyVgKgKsRk7fYtQf0tIJX1DQeUGvxFvD9MWUdCs6J1Zk3urjHm5aAiv3JlCtOOJF1sfQEbgetwS2JOBGrE1R7p_kFd1L_96F2_QVR69FAdb9jn7qEYAiFswX3P4PnctjK6JvyNf9t3ZCSCuWUe21LrLDrozqkwN1vpZFtkAVQdoVwPZpWp9UCO37qDoi086WEB7OL3nrmPOZBP3R8PLyKaRJk37kyVA-6K0HoVpnS0oImFyoedYjB5IagfHY31KHyIIbxnO81H3Zfg0jtKj-';

//defines a test suite
describe('Business Unit Test Result', function () {

	this.timeout(15000);
	var results;
	var response;
	var testProjectID;
	var testProjectUpdateID;


//agent1=chai.request.agent("https://localhost:44300/");
agent1 = chai.request.agent("https://costcodevops.azurewebsites.net");

describe('GetAllVertical-Getting all the verticals name from Data Access layer', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetAllVertical')
		      //.get('dashboard/angular/project/views/ProjectUpdates.html')
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response = res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				done();
		       });
    });

    it('Should return a json string', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

    it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('Key');
					expect(result[i]).to.have.property('Value');
					
				}
				return true;
			});
		done();
		
    })

    it('Json Array in the entry has Known key value types', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					 var jsonkey = result[i].Key;
					 var jsonval = result[i].Value;
					 assert.isNumber(jsonkey, 'value check integer');
					 assert.isString(jsonval, 'value check string');
					
				}
				return true;
			});
		done();
		
    })
   
describe('IsLogin - Checks if the user is logged in or not and creates session for an authorized users', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Account/IsLogin')	
		      .set('Cookie',cookie)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = res.text;
				done();
		       });
    });

    it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });


    it('Should return a logged in user Id which is a string', function(done){
		assert.isString(result, 'value check string');
		done();
    });    


});

});

describe('GetVerticalProjects-get all the projects for specific vertical from data access layer', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/Vertical/GetVerticalProjects/0')
		      .set('Cookie', cookie)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				testProjectID=result[0].ProjectID;
				//console.log(res.text);
				done();
		       });
    });
    it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });
     it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('LatestUpdate');
					expect(result[i]).to.have.property('ProjectID');
					expect(result[i]).to.have.property('ProjectName');
					expect(result[i]).to.have.property('Description');
					expect(result[i]).to.have.property('VerticalID');
					expect(result[i]).to.have.property('Vertical');
					expect(result[i]).to.have.property('ProjectPhases');
					expect(result[i]).to.have.property('ProjectUpdates');
					expect(result[i]).to.have.property('StatusUpdates');
					
				}
				return true;
			});
		done();		
    });

it('Should return a Json Array has know key value types', function(done){
	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					 assert.isString(result[i].LatestUpdate, 'value check string');
					 assert.isString(result[i].ProjectID, 'value check string');
					 assert.isString(result[i].ProjectName, 'value check string');
					  assert.isNull(result[i].VerticalID, 'value check Null');					
					 assert.isNull(result[i].Description, 'value check Null');
					 assert.isNull(result[i].Vertical, 'value check Null');
					 assert.isArray(result[i].ProjectPhases, 'Value check isArray');
					 assert.isArray(result[i].ProjectUpdates, 'value check isArray');
					 assert.isArray(result[i].StatusUpdates, 'value check isArray');					  

				}
				return true;
			});
		done();


	
       
    });
	

});


describe('GetProjectUpdates-Get all information about specific project from GetProjectUpdates on Data Access layer based on the projectID', function() {
this.timeout(15000);
    it('returns', function(done) {    	 
		return agent1
		      .get('/ProjectList/GetprojectUpdates/'+testProjectID)
		      .set('Cookie',cookie)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				testProjectUpdateID = result[0].ProjectUpdateID;
				//console.log(testProjectUpdateID);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.below(2);
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

     it('Should return known Keys',function(done){
     	expect(result[0]).to.include.keys('$id');
		expect(result[0]).to.include.keys('Date');
		expect(result[0]).to.include.keys('Environment');
		expect(result[0]).to.include.keys('Description');
		expect(result[0]).to.include.keys('PhaseID');
		expect(result[0]).to.include.keys('ProjectUpdateID');
		expect(result[0]).to.include.keys('ProjectID');
		expect(result[0]).to.include.keys('Subject');
		expect(result[0]).to.include.keys('Project');
		expect(result[0]).to.include.keys('StatusUpdates');			
		done();
     })

     it('Should return an array of Know Key Value types', function(done){
     	assert.isString(result[0].$id, 'value check string');
		assert.isString(result[0].Date, 'value check string');
		assert.isString(result[0].Environment, 'value check Null');
		assert.isString(result[0].Description, 'value check Null');					
		assert.isNumber(result[0].PhaseID, 'value check Number');
		assert.isString(result[0].ProjectUpdateID, 'value check string');
		assert.isString(result[0].ProjectID, 'Value check string');
		assert.isString(result[0].Subject, 'value check string');
		assert.isObject(result[0].Project, 'value check object');
		assert.isArray(result[0].StatusUpdates, 'value check object');		
		done();		
     });      
});


describe('GetStatusData-Get the project information from GetAllUpdatesFromEmail in Data Access layer', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .get('/ProjectList/GetStatusData/'+testProjectID+'/'+ testProjectUpdateID)
		      .set('Cookie',cookie)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = JSON.parse(JSON.stringify(eval(res.text)));
				//console.log(result);
				done();
		       });
    });

     it('Should return a json string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

     it('Json Array in the entry has Known Keys', function(done){
    	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					expect(result[i]).to.have.property('PhaseID');
					expect(result[i]).to.have.property('ProjectID');
					expect(result[i]).to.have.property('ProjectName');
					expect(result[i]).to.have.property('ProjectUpdateID');
					expect(result[i]).to.have.property('VerticalID');
					expect(result[i]).to.have.property('RecordDate');
					expect(result[i]).to.have.property('UpdateKey');
					expect(result[i]).to.have.property('UpdateValue');
					expect(result[i]).to.have.property('Phase');
					expect(result[i]).to.have.property('Project');
					expect(result[i]).to.have.property('ProjectUpdate');
					expect(result[i]).to.have.property('Vertical');	
					           					
				}
				return true;
			});
           
		done();		
    });

     it('Should return a Json Array has know key value types', function(done){
	expect(result).to.satisfy(
			function (result) {
				for (var i = 0; i < result.length; i++) {
					 assert.isNumber(result[i].PhaseID, 'value check Number');
					 assert.isString(result[i].ProjectID, 'value check string');
					 assert.isString(result[i].ProjectName, 'value check string');
					 assert.isString(result[i].ProjectUpdateID, 'value check string');
					 assert.isNumber(result[i].VerticalID, 'value check Number');					
					 assert.isString(result[i].RecordDate, 'value check string');
					 assert.isString(result[i].UpdateKey, 'value check string');
					 assert.isNull(result[i].Phase, 'value check null');
					 assert.isNull(result[i].Project, 'value check null');
					 assert.isNull(result[i].ProjectUpdate, 'value check null');
					 assert.isNull(result[i].Vertical, 'value check null');									  

				}
				return true;
			});
		done();	
       
    });


});

var emailJson = {
"ProjectName":"MochaTest6:Ecomm_NonCore",

"Subject":"Deployment Succeeded wdc_prod_group1: Ecomm_NonCore [RxWebInt_3.0.4877.0]",

"Body":"Execution of a process on Ecomm_NonCore has completed successfully:\r\n\r\nApplication:\r\n\r\nEcomm_NonCore\r\n\r\nProcess:\r\n\r\nDeploy\r\n\r\nSnapshot:\r\n\r\nRxWeb1603A\r\n\r\nEnvironment:\r\n\r\nwdc_prod_group1\r\n\r\nRequested By:\r\n\r\nmcarmod\r\n\r\nRequested On:\r\n\r\nMon, 18 Apr 2016 15:27:03 -0700\r\n\r\nDescription:\r\nExecution Summary:\r\n\r\n*Process*\r\n\r\n*Resource*\r\n\r\n*Start*\r\n\r\n*Duration*\r\n\r\n*Status*\r\nVersions Included:\r\n\r\n*Component*\r\n\r\n*Version*\r\n\r\n*Description*\r\n\r\nEcomm_RxWeb_InstallSQL\r\n\r\nRxWebInt_3.0.4877.0\r\n\r\nJenkins build: 10 SVN revision: SVN_REVISION_1\r\nNo Inventory Changes Made\r\n\r\nAdditional information available on uDeploy <https://udeploy.costco.com/>:\r\nhttps://udeploy.costco.com#applicationProcessRequest/64c9916a-3ea8-4fba-bdae-e7ff3fcaa72f\r\n<https://udeploy.costco.com/#applicationProcessRequest/64c9916a-3ea8-4fba-bdae-e7ff3fcaa72f>\r\n\r\n",

"Updates":[{"Key":"Application:","Value":"Ecomm_NonCore"},{"Key":"Process:","Value":"Deploy"},{"Key":"Environment:","Value":"wdc_prod_group1"},{"Key":"Requested By:","Value":"mcarmod"},{"Key":"Requested On:","Value":"Mon, 18 Apr 2016 15:27:03 -0700"},{"Key":"Description:","Value":""}]

}

describe('ProjectUpdate-Receives a json object, deserialze the object and pass the inforamtion to the the RecordUpdatePackage', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/ProjectUpdate/Update')
		      .send(emailJson)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result = res.text;
			   // console.log(res.text);
				done();
		       });
    });

     it('Should return a string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });
 	it('Should return a json with known key value types', function(done){
		assert.isString(result,'value check string');
		done();
    });

});

describe('Display - Redirects to home Page', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/ProjectList/Display')		      
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result=res.text
				//console.log(res.text);
				done();
		       });
    });

     it('Should return a string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html; charset=utf-8');
		done();
    });

    it('Should return a json with known key value types', function(done){
		assert.isString(result,'value check string');
		done();
    });
});

describe('Logoff-Ends the current user session', function() {
this.timeout(15000);
    it('returns', function(done) {        
		return agent1
		      .post('/Account/Logoff')
		      .set('Cookie',cookie)
		      .end( function (err, res) {
				expect(err).to.be.null;
				expect(res).to.have.status(200);
				response=res;
				result=res.text;
				//console.log(res.text);
				done();
		       });
    });

     it('Should return a string with valid status and header', function(done){
		expect(response).to.have.status(200);
		expect(result).to.have.length.above(1);	
		expect(response).to.have.header('content-type', 'text/html');
		done();
    });

    it('Should return a json with known key value types', function(done){
		assert.isString(result,'value check string');
		done();
    });


});

});


