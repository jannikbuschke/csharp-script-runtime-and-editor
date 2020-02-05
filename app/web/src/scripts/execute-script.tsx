import * as React from "react"
import {
  Input,
  FormikDebug,
  SubmitButton,
  FormItem,
  ResetButton,
  Form,
} from "formik-antd"
import { Formik } from "formik"
import { message, PageHeader, Button, notification } from "antd"
import { ExecuteScript } from "../models/scripts/ExecuteScript"
import { ControlledEditor } from "@monaco-editor/react"
// import prettier from "prettier";
// import parser from "prettier-plugin-csharp";

export const ExecuteScriptView = (props: any) => {
  return (
    <Formik<ExecuteScript>
      initialValues={{
        script: "",
      }}
      onSubmit={async values => {
        console.log("submit")
        const response = await fetch("/api/scripts/execute?api-version=1.0", {
          method: "POST",
          body: JSON.stringify(values),
          headers: { "content-type": "application/json" },
        })
        if (response.ok) {
          const result = await response.text()
          message.success("result: " + result)
        } else {
          const text = await response.text()
          if (text) {
            notification.error({
              message: response.statusText + ": " + text,
              duration: 0,
            })
          } else {
            message.error("error: " + response.statusText)
          }
        }
      }}
    >
      {f => (
        <Form>
          <div>
            <PageHeader
              title="Execute Script"
              extra={[
                <SubmitButton key="execute">Execute</SubmitButton>,
                <ResetButton key="reset">Reset</ResetButton>,
                // <Button key="format" onClick={()=>{
                //   console.log("foo")
                //   try{
                //     const result = prettier.format(f.values["script"], {
                //       parser: "cs" as any,
                //       plugins: [parser]
                //     });
                //     console.log("result", result)

                //     f.setFieldValue("script", result )
                //   }catch(e){
                //     console.error(e)
                //   }
                // }}>Format</Button>
              ]}
            >
              <ControlledEditor
                height="90vh"
                language="csharp"
                onChange={(e, value) => {
                  f.setFieldValue("script", value)
                }}
                value={f.values["script"]}
              />
              <FormItem name="script" label="Script">
                <Input.TextArea name="script" rows={20} />
              </FormItem>
            </PageHeader>
          </div>
        </Form>
      )}
    </Formik>
  )
}
