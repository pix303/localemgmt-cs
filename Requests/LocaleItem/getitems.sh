#!/bin/bash

curl -X GET http://localhost:5028/localeitem/search \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

curl -X GET http://localhost:5028/localeitem/search?content=ciao&lang=it&context=app\
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

curl -X GET http://localhost:5028/localeitem/search?lang=it&context=app\
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

curl -X GET http://localhost:5028/localeitem/search?content=ciao\
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

curl -X GET http://localhost:5028/localeitem/search?content=xxxxx\
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

# curl -X GET http://localhost:5028/localeitem/detail?aggregateId=596a106d-f8cb-4dd3-b062-a7b40bfc5f92 \
#   --header "Content-Type: application/json" \
#   --data "$json_data"  | jq .

